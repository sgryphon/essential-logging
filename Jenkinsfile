pipeline {
  agent {
    docker {
      image 'mcr.microsoft.com/dotnet/core/sdk:3.1'
    }
  }
  stages {
    stage('Build') {
      steps {
        sh 'pwd'
        sh 'whereis pwsh'
        sh 'echo $PATH'
        pwsh 'Write-Host "Host-PowerShell";'
        pwsh 'build.ps1'
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
  }
}