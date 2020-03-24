pipeline {
  agent {
    docker {
      // export LD_LIBRARY_PATH=/root/.nuget/packages/gitversion.tool/5.1.2/tools/netcoreapp3.0/any/runtimes/debian.9-x64/native/
      image 'mcr.microsoft.com/dotnet/core/sdk:3.1'
    }
  }
  stages {
    stage('Build') {
      steps {
        sh 'mkdir pack'
        sh 'pwsh -File ./build.ps1'
      }
    }

    stage('Test') {
      steps {
        sh 'dotnet test --no-build'
      }
    }

    stage('Package') {
      steps {
        sh 'dotnet pack src/Essential.LogTemplate -c Release -p:WriteVersionInfoToBuildLog=false --output pack'
        sh 'dotnet pack src/Essential.LoggerProvider.RollingFile -p:WriteVersionInfoToBuildLog=false -c Release --output pack'
      }
    }

  }
  environment {
    DOTNET_CLI_HOME = '/tmp'
    LD_LIBRARY_PATH = '/tmp/.nuget/packages/gitversion.tool/5.2.4/tools/netcoreapp3.1/any/runtimes/debian.9-x64/native/'
  }
}