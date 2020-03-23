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
        pwsh '$PSVersionTable'
        sh 'dotnet version'
        sh 'dotnet build'
        pwsh 'dotnet --version'
      }
    }

    stage('Test') {
      steps {
        sh 'dotnet test --no-build'
      }
    }

    stage('Package') {
      steps {
        sh 'dotnet pack src/Essential.LogTemplate -c Release --output pack'
        sh 'dotnet pack src/Essential.LoggerProvider.RollingFile -c Release --output pack'
      }
    }

  }
}