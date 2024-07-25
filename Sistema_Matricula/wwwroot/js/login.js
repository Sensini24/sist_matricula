let dark_light = document.getElementById('dark-light')
let card_login = document.getElementById('card-login')
let button_login = document.getElementById('button-login')
let fields = document.querySelectorAll('.fields')
let remember = document.getElementById('remebmber')
var body = document.querySelector("body")
let frases = document.querySelectorAll(".frases")
let nombre_checkbox = document.querySelector('label#nombre-checkbox')

// Verificar si hay un valor guardado en localStorage
const isDarkMode = localStorage.getItem('darkMode') === 'true';

// Aplicar el modo oscuro si está activado
if (isDarkMode) {
    body.style.background = "#0c0c28";
    card_login.style.background = "#212141";
    button_login.style.background = "#5c59fd";
    fields.forEach(field => {
        field.style.background = "#212141";
    });
    frases.forEach(frase => {
        frase.style.color = "#FFFFFF";
    });

    nombre_checkbox.style.color = "green"
    dark_light.checked = true;
} else {
    body.style.background = "#f2f2ff";
    card_login.style.background = "#FFFFFF";
    button_login.style.background = "#3935ff";
    fields.forEach(field => {
        field.style.background = "#FFFFFF";
    });
    frases.forEach(frase => {
        frase.style.color = "#000000";
    });
    nombre_checkbox.style.color = "black"
    dark_light.checked = false;
}


dark_light.addEventListener('change', function () {

    body.style.background = this.checked ? "#0c0c28" : "#f2f2ff";
    card_login.style.background = this.checked ? "#212141" : "#FFFFFF";
    button_login.style.background = this.checked ? "#5c59fd" : "#3935ff";
    fields.forEach(field => {
        field.style.background = this.checked ? "#212141" : "#FFFFFF";
    })
    frases.forEach(frase => {
        frase.style.color = this.checked ? "#FFFFFF" : "#000000";
    })

    nombre_checkbox.style.color = this.checked ? "#f2f2ff" : "#0c0c28"

    localStorage.setItem('darkMode', this.checked);
});

