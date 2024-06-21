
    const links = document.querySelectorAll('a');

    links.forEach((link) => {
        link.addEventListener('click', (e) => {
            // Aquí puedes hacer algo cuando se hace click en un enlace
            e.preventDefault();

            // Cambia el color del enlace que se ha seleccionado
            link.style.background = 'blue';
            alert("clicado boton")
        });
    });