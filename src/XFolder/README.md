# XFolder 源码

该目录从已发布的 `XFolder.dll` 恢复，因为仓库原先只有 README 和发布截图。项目目标框架为 .NET 10，Avalonia 版本为 12.0.1。

当前项目引用已安装应用中的托管依赖。依赖目录按以下顺序解析：

1. 构建参数 `-p:XFolderBundleDir=...`
2. 仓库内的本地调试副本 `local/XFolder.app/Contents/MonoBundle`
3. 正式安装目录 `/Applications/XFolder.app/Contents/MonoBundle`

构建：

```sh
DOTNET_ROOT=/opt/homebrew/opt/dotnet/libexec \
dotnet build XFolder.csproj -c Release -p:WarningLevel=0
```

如果依赖来自其他应用包：

```sh
DOTNET_ROOT=/opt/homebrew/opt/dotnet/libexec \
dotnet build XFolder.csproj -c Release -p:WarningLevel=0 \
  -p:XFolderBundleDir=/path/to/XFolder.app/Contents/MonoBundle
```

生成文件位于：

```text
bin/Release/net10.0/XFolder.dll
```

## 制作测试应用

不要直接覆盖正在运行的 `/Applications/XFolder.app`。仓库根目录的调试脚本会负责构建、复制到 `local/XFolder.app`、替换 DLL 并重新签名：

```sh
./scripts/XFolder-Debug.command --debug     # 构建 → 打包 → 启动
./scripts/XFolder-Debug.command --status    # 查看构建 / 实例 / 崩溃日志状态
```

注意：每次重新签名都会改变 CDHash，macOS 可能再次弹出「访问桌面文件夹」授权。

如果 macOS 启动缓存仍指向旧包，脚本已经为本地副本设置独立的 `CFBundleIdentifier`（`com.github.zebrapixel.xfolder.debug`）和显示名 `XFolder Debug`。

## 已修复的崩溃

- `FileTreeItem` 和详情列表在文件系统 `Created` 事件到达后，读取已被原子写工具删除的临时文件，触发未捕获的 `FileNotFoundException`。
- `CachingJsonSettingsDatabase` 被多个设置服务并发读取时，缓存初始化存在 `ConcurrentDictionary` 键冲突。
- 磁盘设备事件在后台线程直接遍历界面绑定的 `ObservableCollection`，触发 `InvalidOperation_EnumFailedVersion`。
- 恢复源码中的 ToolTip 附加属性动态绑定错误会在面包屑栏加载时抛出 `InvalidCastException`。

`CompiledAvaloniaXaml/` 与各 View 中的 `XamlClosure_*` 是反编译恢复的编译后 XAML 代码，可读性低于原始 AXAML。后续迭代应优先修改业务类；必须修改这些生成代码时，先确认界面能否正常加载。
