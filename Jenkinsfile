pipeline {
    agent { docker { image 'mcr.microsoft.com/dotnet/core/sdk:3.1' } }
    stages {
        stage('test') {
            steps {
                sh 'dotnet test'
            }
        }
        stage('package') {
            steps {
                sh 'dotnet pack src/Essential.LogTemplate -c Release --output pack'
                sh 'dotnet pack src/Essential.LoggerProvider.RollingFile -c Release --output pack'
            }
        }
    }
}