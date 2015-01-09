$target = $args[0]        # ie "\\testserver\e$"

if (-not($target)) {
    Write-Output "You must provide a deployment location param as second item. Exiting!"
    Exit
}

Copy-Item 'common.config' "${target}\" -Force -Recurse