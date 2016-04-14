Write-Host "This project has this many lines: "
(dir -include *.cs,*.xaml -recurse | select-string .).Count
Write-Host "Press any key to continue..."
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")