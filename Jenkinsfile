pipeline {
  agent {
    docker {
      image 'mcr.microsoft.com/dotnet/core/sdk:3.1'
    }
  }
  
  environment {
    DOTNET_CLI_HOME = "/tmp"
  }
  
  stages {
    stage('Build') {
      steps {
        sh 'pwd'
        sh 'dotnet --version'
        sh 'dotnet build -p:WriteVersionInfoToBuildLog=false'
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
}