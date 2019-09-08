const { desktopCapturer } = require('electron')
const signalR = require('@microsoft/signalr')

let connection;
let subject;
let screenCastTimer;
let isStreaming = false;
const framePerSecond = 10;
const screenWidth = 1280;
const screenHeight = 800;
const apiUrl = "http://localhost:5001/ScreenCastHub";

async function initializeSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl(apiUrl)
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("NewViewer", function () {
        if (isStreaming === false)
            startStreamCast()
    });

    connection.on("NoViewer", function () {
        if (isStreaming === true)
            stopStreamCast()
    });

    await connection.start().then(function () {
        console.log("connected");
    });

    return connection;
}

initializeSignalR();

function CaptureScreen() {
    return new Promise(function (resolve, reject) {
        desktopCapturer.getSources({ types: ['screen'], thumbnailSize: { width: screenWidth, height: screenHeight } },
            (error, sources) => {
                if (error) console.error(error);
                for (const source of sources) {
                    if (source.name === 'Entire screen') {
                        resolve(source.thumbnail.toDataURL())
                    }
                }
            })
    })
}

const agentName = document.getElementById('agentName');
const startCastBtn = document.getElementById('startCast');
const stopCastBtn = document.getElementById('stopCast');
stopCastBtn.setAttribute("disabled", "disabled");

startCastBtn.onclick = function () {
    startCastBtn.setAttribute("disabled", "disabled");
    stopCastBtn.removeAttribute("disabled");
    connection.send("AddScreenCastAgent", agentName.value);
};

function startStreamCast() {
    isStreaming = true;
    subject = new signalR.Subject();
    connection.send("StreamCastData", subject, agentName.value);
    screenCastTimer = setInterval(function () {
        try {
            CaptureScreen().then(function (data) {
                subject.next(data);
            });

        } catch (e) {
            console.log(e);
        }
    }, Math.round(1000 / framePerSecond));
}

function stopStreamCast() {

    if (isStreaming === true) {
        clearInterval(screenCastTimer);
        subject.complete();
        isStreaming = false;
    }
}

stopCastBtn.onclick = function () {
    stopCastBtn.setAttribute("disabled", "disabled");
    startCastBtn.removeAttribute("disabled");
    stopStreamCast();
    connection.send("RemoveScreenCastAgent", agentName.value);
};

