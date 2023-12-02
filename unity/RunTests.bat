ECHO "Running tests..."

set UNITY_EXECUTABLE="E:/Unity/2022.3.4f1/Editor/Unity.exe"
set PROJECT_PATH="E:/Unity/My project"

%UNITY_EXECUTABLE% -runTests -runTests -batchmode -projectPath %PROJECT_PATH% -nographics -testPlatform "PlayMode"