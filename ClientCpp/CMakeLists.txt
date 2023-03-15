cmake_minimum_required (VERSION 3.5)
project (signalrcppclient)

find_package(microsoft-signalr REQUIRED)
link_libraries(microsoft-signalr::microsoft-signalr)

set (SOURCES
  HubConnectionSample.cpp
)

add_executable (HubConnectionSample ${SOURCES})

