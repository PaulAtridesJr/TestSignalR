Install MS VS with C++ support (check CMake)
Add to PATH CMake.exe folder


Install vcpkg

create dir C:\_some_dir_\vcpkg
    git clone https://github.com/microsoft/vcpkg
    .\vcpkg\bootstrap-vcpkg.bat
    .\vcpkg\vcpkg integrate install
add to PATH C:\_some_dir_\vcpkg


Install Microsoft SignalR C++ package with dependencies

    vcpkg install microsoft-signalr:x86-windows
and/or
    vcpkg install microsoft-signalr:x64-windows


Ready to build test project 'ClientCpp'

----------------------------
Build in Visual Studio

// Vcpkg with Visual Studio CMake Projects
// Open the CMake Settings Editor, and under CMake toolchain file, add the path to the vcpkg toolchain file:
// C:/_some_dir_/vcpkg/scripts/buildsystems/vcpkg.cmake

Open folder with ClientCpp
Check HubConnectionSample.cpp for no errors
Build -> Build All

---------------------------
Build in console

C:\_some_dir_\ClientCpp> cmake . -A x64 -DCMAKE_TOOLCHAIN_FILE="C:\_some_dir_\vcpkg\scripts\buildsystems\vcpkg.cmake" -DCMAKE_BUILD_TYPE=Release
C:\_some_dir_\ClientCpp> cmake --build . --config Release