## 环境

- XFolder v1.1.0（macOS arm64 DMG）
- macOS 27.0.1，arm64
- 应用长期运行后会在没有明显操作的情况下退出

## 已定位到的主要崩溃

### 1. 文件监视器处理已消失的临时文件

`crash.log` 中多次出现：

```text
System.IO.FileNotFoundException
  at System.IO.FileInfo.get_Length()
  at Files.Data.Items.FileTreeItem..ctor(...)
  at Files.Data.Items.FileTreeItem.DirectoryWatcher_OnCreated(...)
```

以及：

```text
System.IO.FileNotFoundException
  at System.IO.FileInfo.get_Length()
  at Files.ViewModels.Layouts.BaseLayoutViewModel.FilePath2ListedItem(...)
  at Files.ViewModels.Layouts.BaseLayoutViewModel.DirectoryWatcher_OnCreated(...)
```

触发路径包括 `.claude.json.lock` 和 `*.html.tmp.*`。编辑器与 CLI 常使用“创建临时文件、原子重命名、立即删除”的写入方式，因此 `Created` 事件投递到 UI 线程时，路径可能已经不存在。

建议在 `Created` 回调中重新验证 `File.Exists` / `Directory.Exists`，并让文件、目录到 `ListedItem` 的转换捕获 `IOException`、`UnauthorizedAccessException` 后返回 `null`。临时目录项应直接跳过，不能让异常冒泡到 UI 线程。

### 2. 设置缓存并发初始化

另一类日志为：

```text
System.ArgumentException
  at System.Collections.Concurrent.ConcurrentDictionary...TryAdd
  at ...CachingJsonSettingsDatabase.GetValue(...)
  at ...FoldersSettingsService.ShowThumbnails
```

启动时多个设置服务可能并发访问同一个缓存。建议用单个锁保护缓存初始化、读写和导入，或使用 `Lazy<IDictionary<string, object?>>` / `GetOrAdd` 保证只初始化一次。

### 3. 磁盘设备事件跨线程修改 UI 集合

`DrivesViewModel.Watcher_DeviceModified` 曾在后台线程直接遍历界面绑定的 `ObservableCollection`：

```text
System.InvalidOperationException
  at ...ObservableCollection[...].Enumerator.MoveNext()
  at ...DrivesViewModel.Watcher_DeviceModified(...)
```

建议设备 Added / Removed / Modified 回调都通过 `Dispatcher.UIThread.InvokeAsync` 再访问 `Drives`。

## 本地验证

本地应用上述修复后：

- 主窗口可正常启动。
- 在当前目录连续执行 1000 次临时文件“创建后立即删除”，应用保持运行。
- 测试结束后 `crash.log` 保持 0 字节。
- 测试副本通过 `codesign --verify --deep --strict`。

当前仓库没有公开源码，因此我无法提交真实代码 PR。如果作者愿意提供私有仓库或接收补丁的方式，我可以提交仅包含上述修复的最小改动 PR。
