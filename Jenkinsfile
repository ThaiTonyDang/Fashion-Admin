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
            url: "git@bitbucket.org:dtranthai/fashion-admin.git"
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

    stage('Publish Source') {
        sh "dotnet publish --no-restore --no-build --configuration Release FashionWeb.Admin/FashionWeb.Admin.csproj -o publish"
    }

    def timeCreateFolder = getCurrentDateTime("dd.MM.yyy")
    def deploy_folder = "/media/source/admin/admin-${timeCreateFolder}-${BUILD_NUMBER}"

    stage('Deploying Server') {
        withCredentials([usernamePassword(credentialsId: 'api-server', usernameVariable: 'userName', passwordVariable: 'password')]) {
            def server = [:]
            server.name = "api-server"
            server.host = "192.168.50.44"
            server.allowAnyHosts = true
            server.user = userName
            server.password = password

            stage("Stop Service") {
                sshCommand remote: server, command: "bash -c /media/source/admin/admin-service.sh"
            }

            stage("Create Deploy Folder") {
                sshCommand remote: server, command: "echo Create deploy folder"
                sshCommand remote: server, command: "mkdir -p ${deploy_folder}"
            }
            
            stage("Clean Old Source") {
                sshCommand remote: server, command: "rm -rf /media/source/admin/main/*"
            }

            stage("Send file to deploy folder") {
                sshPut remote: server, from: "publish/", into: "${deploy_folder}"
                sshCommand remote: server, command: "mv ${deploy_folder}/publish/* ${deploy_folder}"
                sshCommand remote: server, command: "rm -rf ${deploy_folder}/publish"
                sshCommand remote: server, command: "cp -rf ${deploy_folder}/* /media/source/admin/main"
                sshPut remote: server, from: "Dockerfile", into: "/media/source/admin/main"
            }

            stage("Build Image") {
                sshCommand remote: server, command: "cd /media/source/admin/main && docker build -t fashion-admin ."
            }

            stage("Running Image") {
                sshCommand remote: server, command: "cd /media/source/admin/main && docker run --restart unless-stopped -d -p 81:81 --name fashion-admin fashion-admin"
            }
        }
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