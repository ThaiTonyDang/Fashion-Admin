import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.TimeZone;

pipeline {
    agent any
    
    parameters {
        string (
                name:'BRANCH',
                defaultValue: 'develop',
                description: 'Branch git'
            )
    }

    stages {
        stage('Start Build') {
            steps {
                script {
                    def currentDateTime = getCurrentDateTime()
                    def user = getCurrentUserInfo().userId
                    // change display build name and description
                    currentBuild.displayName = "#${BUILD_NUMBER}-${BRANCH}"
                    currentBuild.description = "#Deploy by ${user} on ${currentDateTime}"
                }
            }
        }
    }
}


node {
    stage('Pull Source Code') {
        git branch: "${params.BRANCH}",
            credentialsId: "jenkins",
            url: "git@bitbucket.org:dtranthai/fashion-api.git"
    }

    stage('Running Restore Project Package') {
        sh "dotnet restore"
    }

    stage('Running Build Project') {
        sh "dotnet build --no-restore --configuration Release"
    }

    stage('Running Unit Test') {
        sh "dotnet test --no-restore --no-build --configuration Release -v d"
    }
}


def getCurrentDateTime(String typeFormatDate = "EEEE, MMM d, yyyy, h:mm:ss a") {

    def formatDate = new SimpleDateFormat(typeFormatDate, Locale.getDefault())
    formatDate.setTimeZone(TimeZone.getDefault())
    def date = new Date()
    def timeFormat = formatDate.format(date)
    return timeFormat
}

def getCurrentUserInfo() {
    def userCurrent = [:]
    wrap([$class: 'BuildUser']) {
        userCurrent.fullName = "${BUILD_USER}"
        userCurrent.firstName = "${BUILD_USER_FIRST_NAME}"
        userCurrent.userId = "${BUILD_USER_ID}"
        userCurrent.email = "${BUILD_USER_EMAIL}"
    }
    return userCurrent
}