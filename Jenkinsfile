pipeline {
  agent {
    docker {
      image 'mcr.microsoft.com/dotnet/core/sdk:3.1'
    }
  }
  stages {
    stage('Build') {
      steps {
        // Need 'pack' folder as Nuget source
        sh 'mkdir pack'
        sh 'dotnet build'
      }
    }

    stage('Test') {
      steps {
        sh 'dotnet test --no-build -verbosity normal'
      }
    }

    stage('Package') {
      steps {
        // Need full fetch refspec so that the build script can calculate GitVersion
        sh 'git fetch --progress --tags --prune --prune-tags origin +refs/heads/*:refs/remotes/origin/*'
        sh 'pwsh -File ./build.ps1'
      }
    }

  }
  environment {
    // Need to have permission to write in $HOME
    DOTNET_CLI_HOME = '/tmp'
    // Set library path so GitVersion can load native libraries (known bug)
    LD_LIBRARY_PATH = '/tmp/.nuget/packages/gitversion.tool/5.2.4/tools/netcoreapp3.1/any/runtimes/debian.9-x64/native/'
  }
}