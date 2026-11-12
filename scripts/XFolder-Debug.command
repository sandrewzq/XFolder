#!/bin/bash

set -u

ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd -P)"
PROJECT_DIR="$ROOT_DIR/src/XFolder"
LOCAL_APP="$ROOT_DIR/local/XFolder.app"
OFFICIAL_APP="/Applications/XFolder.app"
LOCAL_DLL="$LOCAL_APP/Contents/MonoBundle/XFolder.dll"
LOCAL_PDB="$LOCAL_APP/Contents/MonoBundle/XFolder.pdb"
CRASH_LOG="$LOCAL_APP/Contents/MonoBundle/crash.log"
LOG_DIR="$ROOT_DIR/local/logs"

DEFAULT_DOTNET_ROOT="/opt/homebrew/opt/dotnet/libexec"

info() {
    printf '\n\033[1;34m%s\033[0m\n' "$*"
}

ok() {
    printf '\033[1;32m%s\033[0m\n' "$*"
}

warn() {
    printf '\033[1;33m%s\033[0m\n' "$*"
}

fail() {
    printf '\033[1;31m%s\033[0m\n' "$*" >&2
}

pause_if_interactive() {
    if [[ -t 0 ]]; then
        printf '\n按 Enter 返回菜单...'
        read -r
    fi
}

find_dotnet() {
    if [[ -x "$DEFAULT_DOTNET_ROOT/dotnet" ]]; then
        DOTNET_ROOT="$DEFAULT_DOTNET_ROOT"
        DOTNET_BIN="$DEFAULT_DOTNET_ROOT/dotnet"
        return 0
    fi

    if command -v dotnet >/dev/null 2>&1; then
        DOTNET_BIN="$(command -v dotnet)"
        DOTNET_ROOT="${DOTNET_ROOT:-$(cd "$(dirname "$DOTNET_BIN")/.." && pwd -P)}"
        return 0
    fi

    fail "没有找到 dotnet。请先安装 .NET 10。"
    return 1
}

build_project() {
    find_dotnet || return 1

    info "构建 Release..."
    (
        cd "$PROJECT_DIR" || exit 1
        DOTNET_ROOT="$DOTNET_ROOT" "$DOTNET_BIN" build \
            XFolder.csproj \
            -c Release \
            -p:WarningLevel=0
    )
}

archive_runtime_log() {
    if [[ -f "$LOCAL_APP/debug.log" ]]; then
        mkdir -p "$LOG_DIR"
        mv "$LOCAL_APP/debug.log" \
            "$LOG_DIR/debug-$(date '+%Y%m%d-%H%M%S').log"
    fi
}

set_local_identity() {
    local plist="$LOCAL_APP/Contents/Info.plist"

    /usr/libexec/PlistBuddy \
        -c 'Set :CFBundleIdentifier com.github.zebrapixel.xfolder.debug' \
        "$plist"
    /usr/libexec/PlistBuddy \
        -c 'Set :CFBundleName XFolder Debug' \
        "$plist"

    if ! /usr/libexec/PlistBuddy \
        -c 'Print :CFBundleDisplayName' \
        "$plist" >/dev/null 2>&1; then
        /usr/libexec/PlistBuddy \
            -c 'Add :CFBundleDisplayName string XFolder Debug' \
            "$plist"
    else
        /usr/libexec/PlistBuddy \
            -c 'Set :CFBundleDisplayName XFolder Debug' \
            "$plist"
    fi
}

create_local_app() {
    if [[ -d "$LOCAL_APP" ]]; then
        ok "本地应用已存在：$LOCAL_APP"
        return 0
    fi

    if [[ ! -d "$OFFICIAL_APP" ]]; then
        fail "找不到基础应用：$OFFICIAL_APP"
        return 1
    fi

    info "从正式版创建本地调试副本..."
    mkdir -p "$ROOT_DIR/local"
    ditto "$OFFICIAL_APP" "$LOCAL_APP" || return 1
    archive_runtime_log
    set_local_identity || return 1
    ok "本地应用副本已创建。"
}

package_local_app() {
    local source_dll="$PROJECT_DIR/bin/Release/net10.0/XFolder.dll"
    local source_pdb="$PROJECT_DIR/bin/Release/net10.0/XFolder.pdb"

    create_local_app || return 1

    if [[ ! -f "$source_dll" ]]; then
        fail "构建产物不存在：$source_dll"
        return 1
    fi

    info "更新本地应用 DLL 并重新签名..."
    stop_local_instance
    archive_runtime_log
    cp "$source_dll" "$LOCAL_DLL" || return 1
    if [[ -f "$source_pdb" ]]; then
        cp "$source_pdb" "$LOCAL_PDB" || return 1
    else
        warn "未找到 PDB，继续打包 DLL。"
    fi
    set_local_identity || return 1
    codesign --force --deep --sign - "$LOCAL_APP" || return 1
    codesign --verify --deep --strict --verbose=2 "$LOCAL_APP" || return 1
    ok "本地应用已更新。"
}

local_instance_pids() {
    pgrep -f "$LOCAL_APP/Contents/MacOS/XFolder" 2>/dev/null || true
}

official_instance_pids() {
    pgrep -f "$OFFICIAL_APP/Contents/MacOS/XFolder" 2>/dev/null || true
}

stop_local_instance() {
    local pids
    pids="$(local_instance_pids)"

    if [[ -n "$pids" ]]; then
        info "关闭现有本地调试实例..."
        kill $pids 2>/dev/null || true

        local attempts=0
        while [[ -n "$(local_instance_pids)" && "$attempts" -lt 20 ]]; do
            sleep 0.2
            attempts=$((attempts + 1))
        done
    fi
}

launch_local_app() {
    if [[ ! -d "$LOCAL_APP" ]]; then
        fail "本地应用不存在，请先构建。"
        return 1
    fi

    local official_pids
    official_pids="$(official_instance_pids)"
    if [[ -n "$official_pids" ]]; then
        warn "正式版正在运行：PID $official_pids"
        warn "两个版本共用设置目录，建议先完全退出正式版。"

        if [[ -t 0 ]]; then
            printf '仍要继续启动本地版？[y/N] '
            local answer
            read -r answer
            case "$answer" in
                y|Y|yes|YES) ;;
                *)
                    return 1
                    ;;
            esac
        else
            fail "非交互模式下不会同时启动两个版本。请先退出正式版。"
            return 1
        fi
    fi

    stop_local_instance
    info "启动 XFolder Debug..."
    open "$LOCAL_APP"
    ok "已发送启动请求。"
}

build_and_launch() {
    build_project || return 1
    package_local_app || return 1
    launch_local_app
}

stress_test() {
    local folder="${1:-$HOME/Desktop/sandrew/d2-study}"
    local count="${2:-1000}"

    if [[ -t 0 ]]; then
        printf '测试目录 [%s]：' "$folder"
        local input
        read -r input
        [[ -n "$input" ]] && folder="$input"

        printf '事件数量 [%s]：' "$count"
        read -r input
        [[ -n "$input" ]] && count="$input"
    fi

    if [[ ! -d "$folder" ]]; then
        fail "测试目录不存在：$folder"
        return 1
    fi

    if [[ ! "$count" =~ ^[0-9]+$ ]] || [[ "$count" -lt 1 ]]; then
        fail "事件数量必须是正整数。"
        return 1
    fi

    info "在 $folder 执行 $count 次文件系统竞态测试..."
    python3 - "$folder" "$count" <<'PY'
import os
import sys
import tempfile
from pathlib import Path

folder = Path(sys.argv[1])
count = int(sys.argv[2])
pid = os.getpid()

for index in range(count):
    if index % 3 == 0:
        fd, path = tempfile.mkstemp(
            prefix=f"xfolder-debug-{pid}-",
            suffix=".tmp",
            dir=folder,
        )
        os.write(fd, b"x" * (index % 31))
        os.close(fd)
        os.unlink(path)
    elif index % 3 == 1:
        path = folder / f"xfolder-debug-dir-{pid}-{index}"
        path.mkdir()
        path.rmdir()
    else:
        source = folder / f"xfolder-debug-src-{pid}-{index}.tmp"
        target = folder / f"xfolder-debug-dst-{pid}-{index}.tmp"
        source.write_bytes(b"x")
        os.replace(source, target)
        os.unlink(target)
PY

    local result=$?
    if [[ "$result" -ne 0 ]]; then
        fail "压力测试脚本执行失败。"
        return "$result"
    fi

    sleep 2
    show_crash_status
}

show_crash_status() {
    if [[ ! -f "$CRASH_LOG" ]]; then
        warn "尚未找到 crash.log。"
        return 0
    fi

    local size
    size="$(stat -f '%z' "$CRASH_LOG")"
    if [[ "$size" -eq 0 ]]; then
        ok "crash.log：0 bytes，没有记录到新崩溃。"
    else
        fail "crash.log：$size bytes"
        tail -n 100 "$CRASH_LOG"
    fi
}

show_logs() {
    info "崩溃日志"
    show_crash_status

    if [[ -f "$LOCAL_APP/debug.log" ]]; then
        info "debug.log 最后 100 行"
        tail -n 100 "$LOCAL_APP/debug.log"
    fi

    if [[ -d "$LOG_DIR" ]]; then
        info "历史日志目录：$LOG_DIR"
    fi
}

show_status() {
    info "XFolder 本地调试状态"

    if [[ -f "$PROJECT_DIR/bin/Release/net10.0/XFolder.dll" ]]; then
        ok "Release DLL：已生成"
    else
        warn "Release DLL：尚未生成"
    fi

    if [[ -d "$LOCAL_APP" ]]; then
        ok "本地应用：$LOCAL_APP"
    else
        warn "本地应用：尚未创建"
    fi

    local local_pids official_pids
    local_pids="$(local_instance_pids)"
    official_pids="$(official_instance_pids)"

    if [[ -n "$local_pids" ]]; then
        ok "本地调试实例：PID $local_pids"
    else
        warn "本地调试实例：未运行"
    fi

    if [[ -n "$official_pids" ]]; then
        warn "正式版实例：PID $official_pids"
    fi

    show_crash_status
}

show_menu() {
    cat <<'MENU'

========================================
 XFolder 本地调试
========================================
1. 构建、更新本地应用并启动
2. 仅构建 Release
3. 启动当前本地版本
4. 运行文件系统竞态压力测试
5. 查看崩溃日志
6. 查看本地调试状态
7. 重新创建本地应用副本
0. 退出
MENU
}

menu_loop() {
    while true; do
        show_menu
        printf '请选择：'
        local choice
        read -r choice

        case "$choice" in
            1) build_and_launch ;;
            2) build_project ;;
            3) launch_local_app ;;
            4) stress_test ;;
            5) show_logs ;;
            6) show_status ;;
            7)
                if [[ -d "$LOCAL_APP" ]]; then
                    warn "重建会删除现有本地副本。"
                    printf '确认重建？[y/N] '
                    local answer
                    read -r answer
                    case "$answer" in
                        y|Y|yes|YES)
                            stop_local_instance
                            rm -rf "$LOCAL_APP"
                            create_local_app
                            ;;
                    esac
                else
                    create_local_app
                fi
                ;;
            0|q|Q) break ;;
            *) fail "无效选项。" ;;
        esac

        pause_if_interactive
    done
}

case "${1:-}" in
    --debug|--build-launch)
        build_and_launch
        ;;
    --build)
        build_project
        ;;
    --package)
        build_project && package_local_app
        ;;
    --launch)
        launch_local_app
        ;;
    --stress)
        stress_test "${2:-}" "${3:-1000}"
        ;;
    --logs)
        show_logs
        ;;
    --status)
        show_status
        ;;
    --recreate)
        stop_local_instance
        rm -rf "$LOCAL_APP"
        create_local_app
        ;;
    --help|-h)
        cat <<'HELP'
用法：
  ./scripts/XFolder-Debug.command                 打开交互菜单
  ./scripts/XFolder-Debug.command --build         仅构建
  ./scripts/XFolder-Debug.command --package       构建并更新、签名本地应用
  ./scripts/XFolder-Debug.command --debug         构建、更新、签名并启动
  ./scripts/XFolder-Debug.command --launch        启动本地应用
  ./scripts/XFolder-Debug.command --stress [目录] [次数]
  ./scripts/XFolder-Debug.command --logs          查看崩溃日志
  ./scripts/XFolder-Debug.command --status        查看状态
  ./scripts/XFolder-Debug.command --recreate      重建本地应用副本
HELP
        ;;
    "")
        menu_loop
        ;;
    *)
        fail "未知参数：$1"
        exit 2
        ;;
esac
