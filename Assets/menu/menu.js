document.addEventListener("DOMContentLoaded", function (event) {
    let menu = document.getElementById('menu_sticky');
    let posicionPrincipal = window.pageYOffset;

    const MAX_SCROLL_MENU = 100;

    //EFECTO HIDE MENU ON SCROLL
    //evento que repetidamente evalua las condiciones al momento de hacer scroll
    window.addEventListener('scroll', function () {
        let desplazamientoActual = this.window.pageYOffset;
        
        if (desplazamientoActual > MAX_SCROLL_MENU) {
            menu.style.top = '-100px';
            //la posicionPrincipal es el eje referente para que el menu se oculte o muestre.
            if (posicionPrincipal >= desplazamientoActual) {
                //mostrar el menu
                menu.style.top = '25px';
            } else {
                //ocultarlo
                menu.style.top = '-100px';
            }
            //despues de una condicion, el eje referente es actualizado con el ultimo despalazamiento realizado
            posicionPrincipal = desplazamientoActual;
        }
    });
});