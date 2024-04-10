/*const wrapper = document.querySelector('.wrapper');
const loginLink = document.querySelector('.login-link');
const registerLink = document.querySelector('.register-link');
const btnPopUp = document.querySelector('.btnLogin-popup');
const iconClose = document.querySelector('.icon-close');

registerLink.addEventListener('click', () => {
    wrapper.classList.add('active');
});

loginLink.addEventListener('click', () => {
    wrapper.classList.remove('active');
});

btnPopUp.addEventListener('click', () => {
    wrapper.classList.add('active-popup');
});

iconClose.addEventListener('click', () => {
    wrapper.classList.remove('active-popup');
});*/

const wrapper = document.querySelector('.wrapper');
const loginLink = document.querySelector('.login-link');
const registerLink = document.querySelector('.register-link');
const btnPopUp = document.querySelector('.btnLogin-popup');
const iconClose = document.querySelector('.icon-close');
const editForm = document.querySelector('.edit');
const goBack = document.querySelector('.go-back');

loginLink.addEventListener('click', () => {
    // Agregamos la clase 'active-popup' al wrapper solo cuando se hace clic en el enlace de "Login"
    wrapper.classList.add('active-popup');
});

// El resto del código permanece igual
registerLink.addEventListener('click', () => {
    wrapper.classList.add('active');
});

btnPopUp.addEventListener('click', () => {
    wrapper.classList.add('active-popup');
});

iconClose.addEventListener('click', () => {
    wrapper.classList.remove('active');
    wrapper.classList.remove('active-popup');
});

editForm.addEventListener('click', () => {
    // Agregamos la clase 'active-popup' al wrapper solo cuando se hace clic en el enlace de "Login"
    editForm.classList.add('active');
});

/*goBack.addEventListener('click', () => {
    // Agregamos la clase 'active-popup' al wrapper solo cuando se hace clic en el enlace de "Login"
    editForm.classList.remove('active-popup');
});*/

editForm.forEach(button => {
    button.addEventListener('click', () => {
        // Muestra el formulario de edición cuando se hace clic en el botón "Editar"
        editFormContainer.style.display = 'block';
    });
});

// Para eliminar registro

$('#remove-icon').click(function () {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                title: "Deleted!",
                text: "Your file has been deleted.",
                icon: "success"
            });
        }
    });
});

