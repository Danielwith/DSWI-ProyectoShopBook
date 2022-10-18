
/* Implementacion de Alertas */

let element = document.getElementsByClassName("swal2-styled");

function Alert(titulo,texto,icono) {
    Swal.fire({
        title: titulo,
        text: texto,
        icon: icono, /* ICON: warning, error, success, info, question*/
        buttons: true,
        showCancelButton: true,
        dangerMode: true
    });

    /* Alert Fix Styles */
    for (let i = 0; i < element.length; i++) {
        if (element[i].getAttribute("style") == "display: none;") {
        }
        else {
            element[i].setAttribute("style",
                "height:2.5em !important; line-height: 0em !important; color: white !important")
        }
    }

    element[0].setAttribute("style", "height:2.5em !important; line-height: 0em !important; color: white !important; background-color: #f56a6a !important")

};

function Notification(mensaje) {
    const Toast=Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 5000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    })

    Toast.fire({
        icon: 'success',
        title: mensaje
    })
}

function Delete() {
    Swal.fire({
        title: 'Estas seguro?',
        text: "No podras revertir esto!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire(
                'Eliminado!',
                '',
                'success'
            )

            /* Alert Fix Styles */
            for (let i = 0; i < element.length; i++) {
                if (element[i].getAttribute("style") == "display: none;") {
                }
                else {
                    element[i].setAttribute("style",
                        "height:2.5em !important; line-height: 0em !important; color: white !important")
                }
            }
        }
    })

    /* Alert Fix Styles */
    for (let i = 0; i < element.length; i++) {
        if (element[i].getAttribute("style")=="display: none;") {
        }
        else {
            element[i].setAttribute("style",
                "height:2.5em !important; line-height: 0em !important; color: white !important")
        }
    }

    element[0].setAttribute("style","height:2.5em !important; line-height: 0em !important; color: white !important; background-color: #f56a6a !important")
}

function AlertForm(titulo, texto, icono) {
    Swal.fire({
        title: titulo,
        text: texto,
        icon: icono, /* ICON: warning, error, success, info, question*/
        buttons: true,
        showCancelButton: true,
        dangerMode: true
    });

    /* Alert Fix Styles */
    for (let i = 0; i < element.length; i++) {
        if (element[i].getAttribute("style") == "display: none;") {
        }
        else {
            element[i].setAttribute("style",
                "height:2.5em !important; line-height: 0em !important; color: white !important")
        }
    }

    element[0].setAttribute("style", "height:2.5em !important; line-height: 0em !important; color: white !important; background-color: #f56a6a !important");
    element[0].setAttribute("type","submit")

};
