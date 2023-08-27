//Login
var loginPopup = document.querySelector("#password-popup");
var usernameInput = document.querySelector("#username-txt");
var passwordInput = document.querySelector("#password-txt");
var loginMsgDisplayer = document.querySelector("#login-msg");
function myGreeting() {
    loginPopup.classList.add("hide");
}
var wrongMsg = "*wrong username or password";
async function login() {
    if (usernameInput.value.trim() === "" || passwordInput.value.trim() === "") return;
    //console.log(JSON.stringify({
    //    username: usernameInput.value,
    //    password: passwordInput.value
    //}));
    var response = await fetch("user/login", {
        method: 'POST',
        mode: 'cors',
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {
            'Content-Type': 'application/json'
        },
        redirect: 'follow',
        referrerPolicy: 'no-referrer',
        body: JSON.stringify({
            username: usernameInput.value,
            password: passwordInput.value
        })
    });
    //console.log(response);
    response = await response.json();
    //console.log(response);
    if (response.isSuccess) {
        loginPopup.classList.add("hide-animation-class");
        setTimeout(myGreeting, 1000);
    }
    else {
        loginMsgDisplayer.textContent = wrongMsg;
    }
}
document.querySelector("#login-button").addEventListener("click", function(){  
    login();
});

// Time
function time() {
    var time = new Date();
    var hour = time.getHours();
    var minute = time.getMinutes();
    var second = time.getSeconds();
    if (minute < 10)
        minute = "0" + minute;
    if (second < 10)
        second = "0" + second;
    if (hour < 10)
        hour = "0" + hour;

    var curDay = time.getDate();
    var curMonth = time.getMonth() + 1;
    var curYear = time.getFullYear();
    //document.getElementById("time").innerHTML = curDay + "/" + curMonth + "/" + curYear;
    document.getElementById("time").innerHTML = hour + ":" + minute + ":" + second;
    setTimeout("time()", 1000);
}
time();

// Code to show data
let temperatureDisplayer = document.querySelector("#temperature-displayer");
let humidityDisplayer = document.querySelector("#humidity-displayer");
let light1Btn = document.querySelector("#light1-btn>input");
let light2Btn = document.querySelector("#light2-btn>input");
let engine1Btn = document.querySelector("#engine1-btn>input");
let fan1Btn = document.querySelector("#fan1-btn>input");

let devices = {
    "light1": {
        button: light1Btn
    },
    "light2": {
        button: light2Btn
    },
    "engine1": {
        button: engine1Btn
    },
    "fan1": {
        button: fan1Btn
    },
}

function turnOnTheLight(nameOfLight) {
    var x = document.getElementById(nameOfLight);
    console.log(x);
    x.className = "show";
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000)
}
//Initialize
async function Initialize() {
    var devicesState = await fetch("/api/remote/currentstates");
    devicesState = await devicesState.json();

    Object.keys(devicesState).forEach(function (e) {
        devices[e].button.checked = devicesState[e];
    });
    light1Btn.addEventListener("click", async function () {
        let response = await fetch("api/remote/change", {
            method: 'POST',
            mode: 'cors',
            cache: 'no-cache',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json'
            },
            redirect: 'follow',
            referrerPolicy: 'no-referrer',
            body: JSON.stringify({ light1: light1Btn.checked })
        });
        let result = await response.json();
        if (result["light1"] === "changed") {
            if (light1Btn.checked) {
                console.log("Light 1 On");
            } else {
                console.log("Light 1 Off");
            }
        }
        else {
            console.log("Light 1 is not synchronized");
        }
    });
    light2Btn.addEventListener("click", async function () {
        let response = await fetch("api/remote/change", {
            method: 'POST',
            mode: 'cors',
            cache: 'no-cache',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json'
            },
            redirect: 'follow',
            referrerPolicy: 'no-referrer',
            body: JSON.stringify({ light2: light2Btn.checked })
        });
        let result = await response.json();
        if (result["light2"] === "changed") {
            if (light2Btn.checked) {
                console.log("Light 2 On");
            } else {
                console.log("Light 2 Off");
            }
        }
        else {
            console.log("Light 2 is not synchronized");
        }
    });
    engine1Btn.addEventListener("click", async function () {
        let response = await fetch("api/remote/change", {
            method: 'POST',
            mode: 'cors',
            cache: 'no-cache',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json'
            },
            redirect: 'follow',
            referrerPolicy: 'no-referrer',
            body: JSON.stringify({ engine1: engine1Btn.checked })
        });
        let result = await response.json();
        if (result["engine1"] === "changed") {
            if (engine1Btn.checked) {
                console.log("engine 1 On");
            } else {
                console.log("engine 1 Off");
            }
        }
        else {
            console.log("engine 1 is not synchronized");
        }
    });
    fan1Btn.addEventListener("click", async function () {
        let response = await fetch("api/remote/change", {
            method: 'POST',
            mode: 'cors',
            cache: 'no-cache',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json'
            },
            redirect: 'follow',
            referrerPolicy: 'no-referrer',
            body: JSON.stringify({ fan1: fan1Btn.checked })
        });
        let result = await response.json();
        if (result["fan1"] === "changed") {
            if (fan1Btn.checked) {
                console.log("fan 1 On");
            } else {
                console.log("fan 1 Off");
            }
        }
        else {
            console.log("fan 1 is not synchronized");
        }
    });
    //Update info real-time
    var timer = setInterval(async function () {
        var humidityNow = await fetch("api/humidity?viewmode=realtime");
        humidityNow = await humidityNow.json();
        if (humidityNow.length > 0) {
            humidityDisplayer.textContent = humidityNow[0].value + " %";
        } else {
            humidityDisplayer.textContent = "Unknown";
        }
        var temperatureNow = await fetch("api/temperature?viewmode=realtime");
        temperatureNow = await temperatureNow.json();
        if (temperatureNow.length > 0) {
            temperatureDisplayer.textContent = temperatureNow[0].value + " oC";
        } else {
            temperatureDisplayer.textContent = "Unknown";
        }
    }, 1000);
}
Initialize();
class SpeechRecognitionApi {
    constructor(options) {
        const SpeechToText = window.speechRecognition || window.webkitSpeechRecognition;
        this.speechApi = new SpeechToText();
        this.speechApi.lang = 'vi-VN';
        this.speechApi.continuous = false;
        this.speechApi.interimResults = false;
        this.output = options.output ? options.output : document.createElement('div');
        console.log(this.output)
        this.speechApi.onresult = (event) => {
            console.log(event);
            var resultIndex = event.resultIndex;
            var transcript = event.results[resultIndex][0].transcript;
            console.log('transcript>>', transcript);
            console.log(this.output)
            // Nếu đèn chưa bật và mình nói bật đèn
            if (transcript === 'Bật đèn một.') {
                if (light1Btn.checked == false) {
                    light1Btn.checked = true;
                    turnOnTheLight("light-1");
                    console.log(light1Btn.checked)
                }
                else {
                    // Thong bao la da bat den
                    window.alert('Light 1 is on!');
                }
            }
            if (transcript === 'Bật quạt.') {
                if (fan1Btn.checked == false) {
                    fan1Btn.checked = true;
                    turnOnTheLight("fan-1");
                    console.log(fan1Btn.checked)
                }
                else {
                    // Thong bao la da bat den
                    window.alert('Fan 1 is on!');
                }
            }
            if (transcript === 'Bật động cơ.') {
                if (engine1Btn.checked == false) {
                    engine1Btn.checked = true;
                    turnOnTheLight("engine-1");
                    console.log(engine1Btn.checked)
                }
                else {
                    // Thong bao la da bat den
                    window.alert('Engine 1 is on!');
                }
            }
            if (transcript === 'Bật đèn 2.') {
                if (light2Btn.checked == false) {
                    light2Btn.checked = true;
                    turnOnTheLight("light-2");
                    console.log(light2Btn.checked)
                }
                else {
                    // Thong bao la da bat den
                    window.alert('Light 2 is on!');
                }
            }
            this.output.textContent = transcript;
        }
    }
    init() {
        this.speechApi.start();
    }
    stop() {
        this.speechApi.stop();
    }
}

window.onload = function () {
    var speech = new SpeechRecognitionApi({
        output: document.querySelector('.output')
    })

    document.querySelector('.btn-start').addEventListener('click', function () {
        speech.init();
    })
    document.querySelector('.btn-stop').addEventListener('click', function () {
        speech.stop()
    })
}