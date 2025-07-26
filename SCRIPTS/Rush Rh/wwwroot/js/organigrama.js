 
 //-------------------------------------------------------------------------------------------------ORGANIGRAMA
 
 const overlay = document.getElementById("sweetalert-overlay"); // Elemento de fondo oscuro


//  let mockData = [
//     { id: "1", idPadre: null, idUsuario: "1", nombre: "Juan Carlos Pérez Gómez", puesto: "CEO", idPuesto: "1", descripcionPuesto: "Chief Executive Officer responsible for the overall vision and direction of the company.", departamento: "Administración", idDepartamento: "1", img: "../uploads/fotosPerfil/7e2d3bb0-fa95-4060-b51b-01e7af78c636_Captura de pantalla 2024-11-22 a la(s) 1.08.42 p.m..png", tipo: "empleado", URLDocumentoDescripcionPuesto: "../uploads/fotosPerfil/2f25b1b6-a933-43c0-adea-2c58523f493f_curp.pdf"},
//     { id: "2", idPadre: "1", nombre: "Recursos Humanos", departamento:"Recursos Humanos", idDepartamento: "3" ,img: "", tipo: "departamento"},
//     { id: "3", idPadre: "2", idUsuario: "3", nombre: "María Fernanda Gómez Pérez", puesto: "Lider RH", idPuesto: "3", descripcionPuesto: "Lider de Recursos Humanos, encargado de la gestión del personal y el desarrollo organizacional.", departamento: "Recursos Humanos", idDepartamento: "3", img: "https://randomuser.me/api/portraits/women/2.jpg", tipo: "empleado", URLDocumentoDescripcionPuesto: "https://example.com/lider-rh-description.pdf"},
//     { id: "10", idPadre: "3", idUsuario: "10", nombre: "Vacante", puesto: "Practicante RH", idPuesto: "4", descripcionPuesto: "Practicante de Recursos Humanos, asistiendo en diversas tareas administrativas.", departamento: "Recursos Humanos", idDepartamento: "3", img: "../assets/Icon_ASK.svg", tipo: "empleado", URLDocumentoDescripcionPuesto: "https://example.com/practicante-rh-description.pdf"},
//     { id: "4", idPadre: "3", idUsuario: "4", nombre: "Ana María Martínez Rodríguez", puesto: "Auxiliar", idPuesto: "5", descripcionPuesto: "Auxiliar de Recursos Humanos, apoyando en la gestión de personal.", departamento: "Recursos Humanos", idDepartamento: "3", img: "https://randomuser.me/api/portraits/women/4.jpg", tipo: "empleado", URLDocumentoDescripcionPuesto: "https://example.com/auxiliar-description.pdf"},
//     { id: "5", idPadre: "3", idUsuario: "5", nombre: "Luis Enrique Rodríguez Sánchez", puesto: "Servicios Generales", idPuesto: "6", descripcionPuesto: "Servicios Generales, encargado de la limpieza y mantenimiento de las instalaciones.", departamento: "Recursos Humanos", idDepartamento: "3", img: "https://randomuser.me/api/portraits/men/5.jpg", tipo: "empleado", URLDocumentoDescripcionPuesto: "https://example.com/servicios-generales-description.pdf"},
//     { id: "6", idPadre: "1", idUsuario: "6", nombre: "TI", departamento: "TI", idDepartamento: "4", img: "", tipo: "departamento"},
//     { id: "7", idPadre: "6", idUsuario: "7", nombre: "José Luis Gómez Pérez", puesto: "Lider TI", idPuesto: "8", descripcionPuesto: "Lider de Tecnología de la Información, responsable de la infraestructura tecnológica.", departamento: "TI", idDepartamento: "4", img: "https://randomuser.me/api/portraits/men/2.jpg", tipo: "empleado", URLDocumentoDescripcionPuesto: "https://example.com/lider-ti-description.pdf"},
//     { id: "8", idPadre: "7", idUsuario: "8", nombre: "Sofía Alejandra Fernández Pérez", puesto: "Desarrollador Senior", idPuesto: "9", descripcionPuesto: "Desarrollador Senior, encargado del desarrollo y mantenimiento de software.", departamento: "TI", idDepartamento: "4", img: "../assets/Icon-Perfil-Usuario1.svg", tipo: "empleado", URLDocumentoDescripcionPuesto: "https://example.com/desarrollador-senior-description.pdf"},
//     { id: "9", idPadre: "7", idUsuario: "9", nombre: "Miguel Ángel Sánchez López", puesto: "Analista de software", idPuesto: "10", descripcionPuesto: "Analista de software, responsable del análisis y diseño de sistemas.", departamento: "TI", idDepartamento: "4", img: "https://randomuser.me/api/portraits/men/7.jpg", tipo: "empleado", URLDocumentoDescripcionPuesto: "https://example.com/analista-software-description.pdf"},
//     { id: "11", idPadre: "8", idUsuario: "11", nombre: "Vacante", puesto: "Practicante TI", idPuesto: "11", descripcionPuesto: "Practicante de Tecnología de la Información, asistiendo en diversas tareas de desarrollo.", departamento: "TI", idDepartamento: "4", img: "../assets/Icon_ASK.svg", tipo: "empleado", URLDocumentoDescripcionPuesto: "https://example.com/practicante-ti-description.pdf"},
//     { id: "12", idPadre: "11", idUsuario: "11", nombre: "Vacante", puesto: "Practicante TI", idPuesto: "11", descripcionPuesto: "Practicante de Tecnología de la Información, asistiendo en diversas tareas de desarrollo.", departamento: "TI", idDepartamento: "4", img: "../assets/Icon_ASK.svg", tipo: "empleado", URLDocumentoDescripcionPuesto: "https://example.com/practicante-ti-description.pdf"},
//     { id: "13", idPadre: "12", idUsuario: "11", nombre: "Vacante", puesto: "Practicante TI", idPuesto: "11", descripcionPuesto: "Practicante de Tecnología de la Información, asistiendo en diversas tareas de desarrollo.", departamento: "TI", idDepartamento: "4", img: "../assets/Icon_ASK.svg", tipo: "empleado", URLDocumentoDescripcionPuesto: "https://example.com/practicante-ti-description.pdf"},
// ];   


 // 🔹 Datos de prueba simulando respuesta del backend
        let mockData = [];
        let nodeMap = {};   



        // 🔹 Renderizar el organigrama con datos simulados
        document.addEventListener("DOMContentLoaded", async function () {

            await peticionDatosOrganigrama();

            
        });


        async function peticionDatosOrganigrama(){
            var token = $('input[name="__RequestVerificationToken"]').val(); // Obtener el token CSRF

            $.ajax({
                type: "GET",
                url: `/Organigrama?handler=Organigrama`,  // Ajusta según tu Razor Page
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN", token); // Agregar token CSRF
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(function (data) {
                
                mockData = data.nodes;
                console.log(mockData);

                if (!verificarSiHayDatosMostrarBotonInicial()) {
                    let treeStructure = parseDataToTreant(mockData);
                    renderTree(treeStructure);
                }     
                

            }).fail(function (status, error) {
                messageAlert("Error", "Hubo un problema al obtener el organigrama. Por favor, recarga la pagina.", "error", "Aceptar")
                    .then(() => {
                        overlay.style.display = "none";
                        overlay.style.zIndex = "1060";
                    });
                console.error("Error en la petición:", status, error);
            });
        }


        function verificarSiHayDatosMostrarBotonInicial(){ 
            if (mockData.length === 0) {
                document.getElementById("tree").innerHTML = 
                `    
                <div class="botonesInicio">                                
                    <button class="botonAgregarDepartamento" onclick="abrirFormularioAgregarDepartamento(null)"> 
                        <span class="material-symbols-outlined signoAgregarDepartamento">add_home_work </span> Agregar Departamento
                    </button>
                </div>  
                `;

                let selectDireccion = document.getElementById("direccionOrganigrama");

                if (selectDireccion) {
                    selectDireccion.addEventListener("mousedown", function (event) {
                    // Mostrar el mensaje de alerta
                    Swal.fire({
                        title : "No hay datos",
                        text: "Por favor ingresa datos para cambiar la direccion del organigrama",
                        icon : "info",
                        didOpen: () => {
                            // Mover el modal de SweetAlert2 al contenedor fijo
                            const swalModal = document.querySelector(".swal2-container");
                            const swalContainer = document.getElementById("swal-fixed-container");
                            if (swalModal && swalContainer) {
                                swalContainer.appendChild(swalModal);
                                swalModal.style.position = "absolute";
                                swalModal.style.top = "50%";
                                swalModal.style.left = "50%";
                                swalModal.style.transform = "translate(-50%, -50%)";
                                swalModal.style.width = "100%";
                            }
        
                            document.querySelector(".swal2-container").classList.add("swal2-backdrop-hide");
                            document.querySelector(".swal2-container").classList.remove("swal2-backdrop-show");
        
                            //SI ESTAMOS EN PANTALLA COMPLETA cambiar color de fondo de sweetalert y overlay
                            if (document.fullscreenElement) {
                                document.querySelector(".swal2-container").classList.add("swal2-backdrop-show");
                                document.querySelector(".swal2-container").classList.remove("swal2-backdrop-hide");
                            }
        
                        },
                    });
                
                    // Evitar que se abra el menú del select
                    event.preventDefault();
                    });
                }
                                
                return true;
            }
        }

        
        function parseDataToTreant(data) {

            // Crear nodos con botones
            data.forEach(node => {
                nodeMap[node.id] = {
                    text: { name: node.name },
                    HTMLclass: node.tipo === "departamento" ? "department-container" : "node-style",
                    children: [],
                    innerHTML: `
                        <div class="node-content ${node.nombre == "Vacante" ? `vacante` : ""} " onclick="abrirOffcanvas(${node.id})"}>
                            ${node.tipo === "empleado" ? `
                            <div class="image-wrapper">
                                <img src="${node.img}" alt="Foto de ${node.nombre}" class="node-img">
                            </div>
                            <div class="node-nombre">${node.nombre}</div>
                            <div class="node-puesto">${node.puesto}</div>
                            ` : `
                            <div class="node-nombre">${node.nombre}</div>
                            `}
                             ${esAdmin ? `
                            <button class="menu-btn">
                                <span class="material-symbols-outlined">more_horiz</span>
                            </button>
                            <div class="node-buttons" onclick="event.stopPropagation();">
                                <div class="node-buttons-container">
                                    <button class="botonAgregarEmpleado" onclick="abrirFormularioAgregarEmpleado(${node.id}, ${node.idDepartamento})"> 
                                        <span class="material-symbols-outlined signoAgregarEmpleado">add</span>Agregar Nodo
                                    </button>
                                    <button class="botonAgregarDepartamento" onclick="abrirFormularioAgregarDepartamento(${node.id})"> 
                                        <span class="material-symbols-outlined signoAgregarDepartamento">add_home_work </span> Agregar Departamento
                                    </button>
                                    <button class="botonEliminarEmpleado" onclick="eliminarRama(${node.id})"> 
                                        <span class="material-symbols-outlined signoEliminarEmpleado">delete</span> Eliminar Rama
                                    </button>
                                </div>
                            </div>
                            ` : ''}
                        </div>
                    `,
                };
            });

            let root = null;
            data.forEach(node => {
                if (node.idPadre === null) {
                    root = nodeMap[node.id]; // Nodo raíz
                } else {
                    nodeMap[node.idPadre].children.push(nodeMap[node.id]);
                }
            });

            return root;
        }

        function renderTree(treeData, rootOrientation = "NORTH") {
            let config = {
                chart: {
                    container: "#tree",
                    nodeAlign: "BOTTOM",
                    rootOrientation: rootOrientation, // 🔹 Orientación de la raíz
                    connectors: {
                        type: "step", 
                        style: {
                            "stroke-width": 1,
                            "arrow-end": "block-wide-long" 
                        }
                    },
                    levelSeparation: 100,  // 🔹 Espaciado vertical entre niveles
                    siblingSeparation: 100, // 🔹 Espaciado horizontal entre nodos hermanos
                    subTeeSeparation: 250,  // 🔹 Espaciado entre subárboles
                    node: {
                        collapsable: true,
                        
                    }, 
                    animateOnInit: true, 
                    animation: {
                        nodeAnimation: "easeInOutCubic",  // Efecto de animación
                        nodeSpeed: 500,   // Velocidad de los nodos (ms)
                        connectorsAnimation: "lineal",  
                        connectorsSpeed: 500
                    }
                },
                nodeStructure: treeData,

            };

            new Treant(config);
        }


        
        // Función para abrir el formulario de SweetAlert2
        async function abrirFormularioAgregarEmpleado(nodeId, idDepartamentoPadre) {
            overlay.style.display = "block";
            console.log(idDepartamentoPadre);
     

            Swal.fire({
                title: "Agregar Nodo",
                html: `
                    <div class="agregarNodoContainer">
                        <div>
                            <label>Seleccionar Usuario</label>
                            <select id="swal-usuario" class="swal2-input" required>

                            </select>
                            <input id="swal-nombre" class="swal2-input" placeholder="Nombre" required>
                        </div>

                        <div>
                            <div class="d-flex justify-content-between align-items-center"> <label>Puesto</label> <button class="btn" data-bs-target="#PuestosDepartamentosModal" data-bs-toggle="modal"> Administrar </button> </div>
                            <select id="swal-puesto" class="swal2-input" required>
                            </select>
                            <input id="swal-nuevo-puesto" class="swal2-input" placeholder="Nuevo Puesto" required>
                        </div>


                    </div>
                `,
                showCancelButton: true,
                confirmButtonText: "Guardar",
                didOpen: () => {
                    // Mover el modal de SweetAlert2 al contenedor fijo
                    const swalModal = document.querySelector(".swal2-container");
                    const swalContainer = document.getElementById("swal-fixed-container");
                    if (swalModal && swalContainer) {
                        swalContainer.appendChild(swalModal);
                        swalModal.style.position = "absolute";
                        swalModal.style.top = "50%";
                        swalModal.style.left = "50%";
                        swalModal.style.transform = "translate(-50%, -50%)";
                        swalModal.style.width = "100%";
                    }

                    document.querySelector(".swal2-container").classList.add("swal2-backdrop-hide");
                    document.querySelector(".swal2-container").classList.remove("swal2-backdrop-show");

                    //SI ESTAMOS EN PANTALLA COMPLETA cambiar color de fondo de sweetalert y overlay
                    if (document.fullscreenElement) {
                        document.querySelector(".swal2-container").classList.add("swal2-backdrop-show");
                        document.querySelector(".swal2-container").classList.remove("swal2-backdrop-hide");
                    }


                    cargarOptionUsuariosSelect("swal-usuario");
                    cargarOptionPuestosSegunDepartamento(idDepartamentoPadre, "swal-puesto");


                },
                preConfirm: () => {
                    let usuario = document.getElementById("swal-usuario").value;
                    let nombre = document.getElementById("swal-nombre").value.trim();
                    let puesto = document.getElementById("swal-puesto").value;
                    let nuevoPuesto = document.getElementById("swal-nuevo-puesto").value.trim();
                
                    // Validaciones condicionales
                    if (!usuario && !nombre) {
                        Swal.showValidationMessage("Si no seleccionas un usuario, debes escribir un nombre.");
                        return false;
                    }

                    if (!puesto && !nuevoPuesto) {
                        Swal.showValidationMessage("Debes seleccionar un puesto o escribir un nuevo puesto.");
                        return false;
                    }


                    return {
                        idUsuario: usuario,
                        nombre: nombre,
                        idPuesto: puesto,
                        nuevoPuesto: nuevoPuesto,
                        idDepartamento: idDepartamentoPadre,
                    };
                }
            }).then((result) => {
                overlay.style.display = "none";
                if (result.isConfirmed) {


                    if(result.value.idUsuario == "vacante"){
                        result.value.idUsuario = null;
                        result.value.nombre = "Vacante";
                    }


                    let nuevoNodo = {
                        IdPadre: nodeId || null,
                        IdUsuario: result.value.idUsuario || null,
                        Nombre: result.value.nombre || null,
                        IdPuesto: {
                            Id: result.value.idPuesto || null,
                            Nombre: result.value.nuevoPuesto || null
                        },
                        IdDepartamento: {
                            Id: result.value.idDepartamento || null,
                        },
                        Tipo: "empleado"  
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val(); // Obtener el token CSRF


                    $.ajax({
                        type: "POST",
                        url: `/Organigrama?handler=AgregarNodoEmpleado`,  // Ajusta según tu Razor Page
                        data: JSON.stringify(nuevoNodo),
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader("XSRF-TOKEN", token); // Agregar token CSRF
                        },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json"
                    }).done(function (data) {

                        if(data.success){
                        
                            messageAlert("Agregado", "El nodo empleado ha sido agregado exitosamente", "success", "Aceptar")
                                .then(() => {
                                    overlay.style.display = "none";
                                    overlay.style.zIndex = "1060";    
                                    peticionDatosOrganigrama();
                                });
                        }
                        else {
                            messageAlert("Error", "Hubo un problema al agregar el nodo empleado.", "error", "Aceptar")
                                .then(() => {
                                    overlay.style.display = "none";
                                    overlay.style.zIndex = "1060";
                                });
                        }
    
                    }).fail(function (xhr, status, error) {
                        messageAlert("Error", "Hubo un problema al agregar el nodo empleado.", "error", "Aceptar")
                            .then(() => {
                                overlay.style.display = "none";
                                overlay.style.zIndex = "1060";
                            });
                        console.error("Error en la petición:", status, error);
                    });
                }
            });    
            
            // Mostrar input si se elige "Nuevo Puesto"
            document.getElementById("swal-usuario").addEventListener("change", function () {
                let nuevoUsuarioInput = document.getElementById("swal-nombre");
                if (this.value === "") {
                    nuevoUsuarioInput.style.display = "inline-block";   
                } else {
                    nuevoUsuarioInput.style.display = "none";
                    nuevoUsuarioInput.value = "";
                }
            });


            // Mostrar input si se elige "Nuevo Puesto"
            document.getElementById("swal-puesto").addEventListener("change", function () {
                let nuevoPuestoInput = document.getElementById("swal-nuevo-puesto");
                if (this.value === "") {
                    nuevoPuestoInput.style.display = "inline-block";
                } else {
                    nuevoPuestoInput.style.display = "none";
                    nuevoPuestoInput.value = "";
                }
            });

                
        }




        // 🔹 Función para agregar empleados o departamentos
        function abrirFormularioAgregarDepartamento(parentId) {
            overlay.style.display = "block";

            fetch("/Organigrama?handler=Departamentos")
                .then(response => response.json())
                .then(data => {
                    let departamentosOptions = data.departamentos.map(d => `<option value="${d.id}">${d.nombre}</option>`).join('');

                    Swal.fire({
                        title: "Agregar Departamento",
                        html: `
                            <div class="agregarNodoContainer">
                                <div>
                                    <div class="d-flex justify-content-between align-items-center"> <label>Departamento</label> <button class="btn" data-bs-target="#PuestosDepartamentosModal" data-bs-toggle="modal"> Administrar </button> </div>
                                    <select id="swal-departamento" class="swal2-input" required>
                                        <option value="">-- Nuevo Departamento --</option>
                                        ${departamentosOptions}
                                    </select>
                                    <input id="swal-nuevo-departamento" class="swal2-input" placeholder="Nuevo Departamento" required>
                                </div>
                            </div>
                        `,
                        showCancelButton: true,
                        confirmButtonText: "Guardar",
                        didOpen: () => {
                            // Mover el modal de SweetAlert2 al contenedor fijo
                            const swalModal = document.querySelector(".swal2-container");
                            const swalContainer = document.getElementById("swal-fixed-container");
                            if (swalModal && swalContainer) {
                                swalContainer.appendChild(swalModal);
                                swalModal.style.position = "absolute";
                                swalModal.style.top = "50%";
                                swalModal.style.left = "50%";
                                swalModal.style.transform = "translate(-50%, -50%)";
                                swalModal.style.width = "100%";

                            }
        
                            document.querySelector(".swal2-container").classList.add("swal2-backdrop-hide");
                            document.querySelector(".swal2-container").classList.remove("swal2-backdrop-show");
        
                            //SI ESTAMOS EN PANTALLA COMPLETA cambiar color de fondo de sweetalert y overlay
                            if (document.fullscreenElement) {
                                document.querySelector(".swal2-container").classList.add("swal2-backdrop-show");
                                document.querySelector(".swal2-container").classList.remove("swal2-backdrop-hide");
                            }
        
                        },
                        preConfirm: () => {
                            let departamento = document.getElementById("swal-departamento").value;
                            let nuevoDepartamento = document.getElementById("swal-nuevo-departamento").value.trim();


                            if (!departamento && !nuevoDepartamento) {
                                Swal.showValidationMessage("Debes seleccionar un departamento o escribir un nuevo departamento.");
                                return false;
                            }
        

                            return {
                                idDepartamento: departamento,
                                nuevoDepartamento: nuevoDepartamento || null
                            };
                        }
                    }).then((result) => {
                        overlay.style.display = "none";

                        if (result.isConfirmed) {
                            let nuevoNodo = {
                                IdPadre: parentId || null,
                                IdDepartamento: {
                                    Id: result.value.idDepartamento || null,
                                    Nombre: result.value.nuevoDepartamento || null
                                },
                                Tipo: "departamento"
                            };


                            var token = $('input[name="__RequestVerificationToken"]').val(); // Obtener el token CSRF

                            $.ajax({
                                type: "POST",
                                url: `/Organigrama?handler=AgregarNodoDepartamento`,  // Ajusta según tu Razor Page
                                data: JSON.stringify(nuevoNodo),
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN", token); // Agregar token CSRF
                                },
                                contentType: "application/json; charset=utf-8",
                                dataType: "json"
                            }).done(function (data) {

                                if(data.success){
                                
                                    messageAlert("Agregado", "El nodo departamento ha sido agregado exitosamente", "success", "Aceptar")
                                        .then(() => {
                                            overlay.style.display = "none";
                                            overlay.style.zIndex = "1060";    
                                            peticionDatosOrganigrama();
                                        });
                                } else {
                                    messageAlert("Error", "Hubo un problema al agregar el nodo departamento.", "error", "Aceptar")
                                        .then(() => {
                                            overlay.style.display = "none";
                                            overlay.style.zIndex = "1060";
                                        });
                                }
            
                            }).fail(function (xhr, status, error) {
                                messageAlert("Error", "Hubo un problema al agregar el nodo departamento.", "error", "Aceptar")
                                    .then(() => {
                                        overlay.style.display = "none";
                                        overlay.style.zIndex = "1060";
                                    });
                                console.error("Error en la petición:", status, error);
                            });
                
                        }
                    });    
                    

                    // Mostrar input si se elige "Nuevo Departamento"
                    document.getElementById("swal-departamento").addEventListener("change", function () {
                        let nuevoDeptoInput = document.getElementById("swal-nuevo-departamento");
                        if (this.value === "") {
                            nuevoDeptoInput.style.display = "inline-block";
                        } else {
                            nuevoDeptoInput.style.display = "none";
                            nuevoDeptoInput.value = "";
                        }
                    });
    
                });
        }



        

        function eliminarRama(nodeId) {
            overlay.style.display = "block";

            Swal.fire({
                title: "¿Eliminar esta rama?",
                text: "Se eliminará el nodo junto con sus hijos. Esta acción no se puede deshacer.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Eliminar",
                cancelButtonText: "Cancelar",
                didOpen: () => {
                    // Mover el modal de SweetAlert2 al contenedor fijo
                    const swalModal = document.querySelector(".swal2-container");
                    const swalContainer = document.getElementById("swal-fixed-container");
                    if (swalModal && swalContainer) {
                        swalContainer.appendChild(swalModal);
                        swalModal.style.position = "absolute";
                        swalModal.style.top = "50%";
                        swalModal.style.left = "50%";
                        swalModal.style.transform = "translate(-50%, -50%)";
                        swalModal.style.width = "100%";

                    }

                    document.querySelector(".swal2-container").classList.add("swal2-backdrop-hide");
                    document.querySelector(".swal2-container").classList.remove("swal2-backdrop-show");

                    //SI ESTAMOS EN PANTALLA COMPLETA cambiar color de fondo de sweetalert y overlay
                    if (document.fullscreenElement) {
                        document.querySelector(".swal2-container").classList.add("swal2-backdrop-show");
                        document.querySelector(".swal2-container").classList.remove("swal2-backdrop-hide");
                    }

                },
            }).then((result) => {
                overlay.style.display = "none";
 
                
                if (result.isConfirmed) {
                    //Completamente seguro de eliminar
                    swal.fire({
                        title: "¿Estás completamente seguro?",
                        text: "¡No podrás revertir esto!",
                        icon: "warning",
                        showCancelButton: true,
                        confirmButtonText: "¡Sí, elimínalo!",
                        cancelButtonText: "Cancelar",
                        confirmButtonColor: "#dc3545",
                        didOpen: () => {
                            // Mover el modal de SweetAlert2 al contenedor fijo
                            const swalModal = document.querySelector(".swal2-container");
                            const swalContainer = document.getElementById("swal-fixed-container");
                            if (swalModal && swalContainer) {
                                swalContainer.appendChild(swalModal);
                                swalModal.style.position = "absolute";
                                swalModal.style.top = "50%";
                                swalModal.style.left = "50%";
                                swalModal.style.transform = "translate(-50%, -50%)";
                                swalModal.style.width = "100%";
        
                            }
        
                            document.querySelector(".swal2-container").classList.add("swal2-backdrop-hide");
                            document.querySelector(".swal2-container").classList.remove("swal2-backdrop-show");
        
                            //SI ESTAMOS EN PANTALLA COMPLETA cambiar color de fondo de sweetalert y overlay
                            if (document.fullscreenElement) {
                                document.querySelector(".swal2-container").classList.add("swal2-backdrop-show");
                                document.querySelector(".swal2-container").classList.remove("swal2-backdrop-hide");
                            }
        
                        },
                    }).then((result) => {

                        if (result.isConfirmed) {
                            var token = $('input[name="__RequestVerificationToken"]').val(); // Obtener el token CSRF

                            // Lista de nodo actual y nodos hijos que se tendrán que eliminar
                            let nodos = [];
                            nodos.push(nodeId);
                            let nodosHijos = obtenerTodosNodosHijos({ id: nodeId });
                            nodos = nodos.concat(nodosHijos.map(n => n.id)); 

                            $.ajax({
                                type: "POST",
                                url: `/Organigrama?handler=EliminarRama`,  // Ajusta según tu Razor Page
                                data: JSON.stringify(nodos),
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN", token); // Agregar token CSRF
                                },
                                contentType: "application/json; charset=utf-8",
                                dataType: "json"
                            }).done(function (data) {

                                if (data.success) {
                                
                                    messageAlert("Eliminado", "La rama ha sido eliminada exitosamente", "success", "Aceptar")
                                        .then(() => {
                                            overlay.style.display = "none";
                                            overlay.style.zIndex = "1060";    
                                            peticionDatosOrganigrama();
                                        });

                                } else {
                                    messageAlert("Error", "Hubo un problema al eliminar la rama.", "error", "Aceptar")
                                        .then(() => {
                                            overlay.style.display = "none";
                                            overlay.style.zIndex = "1060";
                                        });
                                }
            
                            }).fail(function (xhr, status, error) {
                                messageAlert("Error", "Hubo un problema al eliminar la rama.", "error", "Aceptar")
                                    .then(() => {
                                        overlay.style.display = "none";
                                        overlay.style.zIndex = "1060";
                                    });
                                console.error("Error en la petición:", status, error);
                            });
                        }
                    });

                }
            });


        }


        function obtenerTodosNodosHijos(nodo) {
            let nodosHijos = [];
            let nodos = mockData.filter(n => n.idPadre === nodo.id);
        
            nodos.forEach(n => {
                nodosHijos.push(n);
                nodosHijos = nodosHijos.concat(obtenerTodosNodosHijos(n));
            });
        
            return nodosHijos;
        }


        function tieneHijosNodo(idNodo) {
            return mockData.some(n => n.idPadre == idNodo);
        }
        

            
        async function abrirOffcanvas(nodeId) {
            const node = mockData && mockData.find(n => n.id === nodeId);
            let idOffCanvasHtml;
            let contenedorPadre;

        
            if (node) {

                if (node.tipo === 'empleado') {
                    modoNormalDetallesOffCanvas("formDepartamentoOffCanvas");
                    modoNormalDetallesOffCanvas("formEmpleadoOffCanvas");

                    // Extraer la información del nodo
                    const id = node.id;
                    const idImagen = node.idImg || 0;
                    const imgSrc = node.img || "";
                    let idUsuario = node.idUsuario || "";
                    const nombre = node.nombre || "";
                    const idPuesto = node.idPuesto || "";
                    const description = node.descripcionPuesto || "No disponible";
                    const idDepartamento = node.idDepartamento  || "";
                    const URLDocumentoDescripcionPuesto = node.urlDocumentoDescripcionPuesto || "";


                    //Buscar el jefe directo
                    let idPadre = node.idPadre;
                    let encontrado = false;
                    let jefe;
                    while(!encontrado) {
                        if (!idPadre) {
                            encontrado = true;
                            jefe = null;
                            break;
                        }
                        jefe = mockData.find(n => n.id === idPadre);
                        if (jefe.tipo == 'departamento') {
                            idPadre = jefe.idPadre;
                        } else {
                            encontrado = true;
                            idJefe = jefe.id;
                        }
                    }
                    const jefeId = jefe ? jefe.id : 0;

                   

                    await cargarOptionUsuariosSelect("offcanvasNombre");
                    await cargarOptionDepartamentosSelect("offcanvasDepartamento");
                    await cargarOptionPuestosSegunDepartamento(idDepartamento, "offcanvasPuesto");
                    

                    // Agrupar opciones por departamentos
                    let departamentos = {};
                    mockData.forEach(n => {
                        if (!departamentos[n.departamento]) {
                            departamentos[n.departamento] = [];
                        }
                        departamentos[n.departamento].push(n);
                    });

                    let jefesOptions = Object.keys(departamentos).map(departamento => {
                        return `
                            <optgroup label="${departamento}">
                                ${departamentos[departamento].map(n => {
                                    return `<option value="${n.id}" ${n.id === jefeId ? 'selected' : ''} data-idDepartamento="${n.idDepartamento}" > ${n.tipo == 'departamento' ? 'Departamento: ' : ''}  ${n.nombre} ${n.nombre == 'Vacante' ? '(' + n.puesto + ')' : ''}</option>`;
                                }).join('')}
                            </optgroup>
                        `;
                    }).join('');   
                    
                    document.getElementById("offcanvasJefeDirecto").innerHTML = jefesOptions;


                    // Actualizar el contenido del offcanvas
                    document.getElementById('offCanvasIdOrganigramaEmpleado').value = id;
                    document.getElementById('offcanvasImgEmpleado').src = imgSrc;

                    if(nombre == "Vacante"){
                        idUsuario = 0;
                    }
                    
                    //Para usuarios nuevos
                    if(idUsuario === ""){
                        document.getElementById('offcanvasNombreNuevo').value = nombre;
                        document.getElementById('offcanvasNombre').style.display = "none";
                        document.getElementById('offcanvasNombreNuevo').style.display = "block";
                    } else {
                        document.getElementById('offcanvasNombreNuevo').value = "";
                        document.getElementById('offcanvasNombre').style.display = "block";
                        document.getElementById('offcanvasNombreNuevo').style.display = "none";
                    }
                    
                    setSelectValue('offcanvasNombre', idUsuario);

                    setSelectValue('offcanvasPuesto', idPuesto);
                    document.getElementById('offcanvasDescripcionPuesto').value = description;
                    document.getElementById('offcanvasDepartamento').value = idDepartamento;
                    document.getElementById('documentoFotoPerfilEmpleado_Id').value = idImagen;

                    if(!URLDocumentoDescripcionPuesto){
                        document.getElementById('infoAdicional').style.display = "none";
                    } else {
                        document.getElementById('offcanvasDescripcionPuestoPDF').href = URLDocumentoDescripcionPuesto;
                        document.getElementById('infoAdicional').style.display = "block";
                    }


                    if (jefeId)
                        document.getElementById('offcanvasJefeDirecto').value = jefeId;
                    else {
                        //Agregar un option al select
                        let option = document.createElement("option");
                        option.text = "Sin jefe directo";
                        option.value = "sinJefeDirecto";
                        document.getElementById('offcanvasJefeDirecto').appendChild(option);
                        document.getElementById('offcanvasJefeDirecto').value = "sinJefeDirecto";
                    }


                    document.getElementById("offCanvasIdPadre").value = node.idPadre;


                    idOffCanvasHtml = "nodeModal";
                    contenedorPadre = "node-style";



                } else if (node.tipo === 'departamento') {

                    modoNormalDetallesOffCanvas("formDepartamentoOffCanvas");
                    modoNormalDetallesOffCanvas("formEmpleadoOffCanvas");
                    // Extraer la información del nodo
                    const id = node.id;
                    const idImagen = node.idImg || 0;
                    const idDepartamento = node.idDepartamento  || "";
                    const imgSrc = node.img || "";

                    await cargarOptionDepartamentosSelect("offcanvasNombreDepartamento");

        
                    // Actualizar el contenido del offcanvas
                    document.getElementById('offcanvasIdOrganigramaDepartamento').value = id;
                    document.getElementById('offcanvasNombreDepartamento').value = idDepartamento;
                    document.getElementById('offcanvasImgDepartamento').src = imgSrc;
                    document.getElementById('documentoFotoPerfilDepartamento_Id').value = idImagen;


        
                    idOffCanvasHtml = "DepartamentoModal";
                    contenedorPadre = "department-container";
                }
        
                // Cerrar el offcanvas anterior si existe
                if (window.selectedNodeTipo) {
                    const offcanvasAnterior = bootstrap.Offcanvas.getInstance(document.getElementById(window.selectedNodeTipo));
                    if (offcanvasAnterior) {
                        // Esperar a que el offcanvas anterior se cierre antes de abrir el nuevo
                        offcanvasAnterior.hide();
                        document.getElementById(window.selectedNodeTipo).addEventListener('hidden.bs.offcanvas', () => {
                            abrirNuevoOffcanvas(node, idOffCanvasHtml, contenedorPadre);
                        }, { once: true });
                        return; // Salir de la función para esperar el cierre
                    }
                }
        
                // Si no hay offcanvas anterior, abrir el nuevo directamente
                abrirNuevoOffcanvas(node, idOffCanvasHtml, contenedorPadre);
                
            }
        }


        //detectar si se cerro offcanvas


        function setSelectValue(idSelect, value) {
            let select = document.getElementById(idSelect);
            let stringValue = String(value); // Convertimos el valor a string
        
            let optionExists = [...select.options].some(option => option.value === stringValue);
        
            if (optionExists) {
                select.value = stringValue;
            } else {
                console.warn(`El valor ${value} no existe en el select ${idSelect}`);
            }
        }
        

        
        // Función para abrir el nuevo offcanvas
        function abrirNuevoOffcanvas(node, idOffCanvasHtml, contenedorPadre) {
            // Añadir clase a la tarjeta seleccionada
            if (window.selectedNode) {
                window.selectedNode.classList.remove('nodoSeleccionado');
            }
        
            const currentNode = document.querySelector(`.node-content[onclick="abrirOffcanvas(${node.id})"]`).closest('.' + contenedorPadre);
            currentNode.classList.add('nodoSeleccionado');
            window.selectedNode = currentNode;
            window.selectedNodeTipo = idOffCanvasHtml;
        
            // Añadir evento para quitar clase al cerrar el offcanvas
            const offcanvasElement = document.getElementById(idOffCanvasHtml);
            offcanvasElement.addEventListener('hidden.bs.offcanvas', () => {
                currentNode.classList.remove('nodoSeleccionado');
                window.selectedNode = null; // Limpiar la variable
                window.selectedNodeTipo = null; // Limpiar la variable
            }, { once: true });
        
            // Mostrar el offcanvas
            const offcanvas = new bootstrap.Offcanvas(document.getElementById(idOffCanvasHtml));
            offcanvas.show();
        }

        


        function changeDirection(direccion){

            //Dependiendo de la rootOrientation que eligaa el usuario en un select, se le asigna un valor a la variable rootOrientation
            let rootOrientation = direccion;
            let treeStructure = parseDataToTreant(mockData);

            renderTree(treeStructure, rootOrientation);

        }



        function modoEditarDetallesOffCanvas(idFormulario){
            const offcanvas = document.querySelector('.offcanvas.show');
            if (offcanvas && offcanvas.contains(document.getElementById(idFormulario))) {
                formEditar = document.getElementById(idFormulario);
            } else {
                formEditar = null;
            }

            if(formEditar != null){
                if(idFormulario == "formDepartamentoOffCanvas"){
                    modoNormalDetallesOffCanvas("formEmpleadoOffCanvas");
                    //quitar disabled de el input nombre
                    formEditar.offcanvasNombreDepartamento.removeAttribute("disabled");
                    botonEditar = document.getElementById("btnEditarDepartamentoOffCanvas");
                    if(botonEditar.textContent == "Editar"){
                        // Store original values
                        formEditar.originalValues = {
                            nombre: formEditar.offcanvasNombreDepartamento.value,
                            imgSrc: document.getElementById('offcanvasImgDepartamento').src
                        };
                        botonEditar.textContent = "Cancelar";
                        botonEditar.style.backgroundColor = "#dc3545";
                        document.getElementById("btnEditarFotoPerfilDepartamento").style.display = "flex";
                        document.getElementById("btnSubmitDepartamentoOffCanvas").style.display = "block";
                        document.querySelectorAll(".ocultoAlEditar").forEach(element => {
                            element.style.display = "none";
                        });


                        if (formEditar.offcanvasNombreDepartamento.value == "") {
                            document.getElementById("offcanvasNombreDepartamentoNuevo").style.display = "block";
                        }

                    } else {
                        modoNormalDetallesOffCanvas("formDepartamentoOffCanvas");
                    }
                } else {
                    modoNormalDetallesOffCanvas("formDepartamentoOffCanvas");


                    formEditar.offcanvasNombre.removeAttribute("disabled");
                    formEditar.offcanvasPuesto.removeAttribute("disabled");
                    formEditar.offcanvasJefeDirecto.removeAttribute("disabled");
                    formEditar.offcanvasNombreNuevo.removeAttribute("disabled");

                    botonEditar = document.getElementById("btnEditarEmpleadoOffCanvas");
                    if(botonEditar.textContent == "Editar"){     
                        
                        
                        //Comporbar si el enlace estaba oculto
                        enlaceEstaOculto = document.getElementById('infoAdicional').style.display == "none" ? true : false;
                        // Store original values
                        formEditar.originalValues = {
                            nombre: formEditar.offcanvasNombre.value,
                            puesto: formEditar.offcanvasPuesto.value,
                            departamento: formEditar.offcanvasDepartamento.value,
                            jefeDirecto: formEditar.offcanvasJefeDirecto.value,
                            imgSrc: document.getElementById('offcanvasImgEmpleado').src ,
                            idPadre: formEditar.offCanvasIdPadre.value,
                            enlaceEstaOculto: enlaceEstaOculto
                            
                        };
                        if (document.getElementById('offcanvasNombreNuevo')) {
                            formEditar.originalValues.nombreNuevo = formEditar.offcanvasNombreNuevo.value;
                        }


                        document.getElementById("btnPopoverJefeDirecto").style.display = "block";
                        botonEditar.textContent = "Cancelar";
                        botonEditar.style.backgroundColor = "#dc3545";
                        document.querySelectorAll(".ocultoAlEditar").forEach(element => {
                           element .style.display = "none";
                        });
                        document.getElementById("btnEditarFotoPerfilEmpleado").style.display = "flex";
                        document.getElementById("btnSubmitEmpleadoOffCanvas").style.display = "block";

                        if (formEditar.offcanvasNombre.value == "") {
                            document.getElementById("offcanvasNombre").style.display = "block";
                        }

                        //descativar la opcion del select de sin jefe directo y seleccionar el nodo padre
                        let optionSinJefe = document.getElementById('offcanvasJefeDirecto').querySelector('option[value="sinJefeDirecto"]');
                        if(optionSinJefe){
                            optionSinJefe.remove();
                        }
                        formEditar.offcanvasJefeDirecto.value = formEditar.offCanvasIdPadre.value;



                        //Cambiamos el orden de los elementos, para que el jefe salga primero que el departamento y puesto
                        const jefeDirecto = document.getElementById("offcanvasJefeDirecto").parentNode;
                        const departamento = document.getElementById("offcanvasDepartamento").parentNode;
                        
                        departamento.parentNode.insertBefore(jefeDirecto, departamento);

                    } else {
                        modoNormalDetallesOffCanvas("formEmpleadoOffCanvas");
                    }
                }
            }
        }



        async function modoNormalDetallesOffCanvas(idFormulario){
            const offcanvas = document.querySelector('.offcanvas.show');
            formNormal = document.getElementById(idFormulario);
           

            if(formNormal != null){
                if(idFormulario == "formDepartamentoOffCanvas"){
                    formNormal.offcanvasNombreDepartamento.setAttribute("disabled", "true");                  
                    botonEditar = document.getElementById("btnEditarDepartamentoOffCanvas");

                    if(formEditar.originalValues.nombre == ""){
                        document.getElementById("offcanvasNombreDepartamento").style.display = "none";
                        document.getElementById("offcanvasNombreDepartamentoNuevo").style.display = "block";
                    } else {
                        document.getElementById("offcanvasNombreDepartamento").style.display = "block";
                        document.getElementById("offcanvasNombreDepartamentoNuevo").style.display = "none";
                    }

                    if(botonEditar.textContent == "Cancelar"){
                        // Restore original values
                        if(formEditar.originalValues){
                            formNormal.offcanvasNombreDepartamento.value = formEditar.originalValues.nombre;
                            document.getElementById('offcanvasImgDepartamento').src = formEditar.originalValues.imgSrc;
                        }
                        botonEditar.textContent = "Editar";
                        botonEditar.style.backgroundColor = "#2a9d8f";
                        document.getElementById("btnEditarFotoPerfilDepartamento").style.display = "none";
                        document.getElementById("btnSubmitDepartamentoOffCanvas").style.display = "none";
                        document.querySelectorAll(".ocultoAlEditar").forEach(element => {
                            element.style.display = "block";
                        });
                    
                    


                        if(formEditar.originalValues.nombre == ""){
                            document.getElementById("offcanvasNombreDepartamento").style.display = "none";
                        }
                    }
                } else {
                    formNormal.offcanvasNombre.setAttribute("disabled", "true");
                    formNormal.offcanvasPuesto.setAttribute("disabled", "true");
                    formNormal.offcanvasJefeDirecto.setAttribute("disabled", "true");
                    formNormal.offcanvasNombreNuevo.setAttribute("disabled", "true");


                    document.querySelectorAll(".ocultoAlEditar").forEach(element => {
                        element.style.display = "block";
                    });
                    botonEditar = document.getElementById("btnEditarEmpleadoOffCanvas");



                    if (formEditar.originalValues.nombre == "") {
                        document.getElementById("offcanvasNombre").style.display = "none";
                        document.getElementById("offcanvasNombreNuevo").style.display = "block";
                    } else {
                        document.getElementById("offcanvasNombre").style.display = "block";
                        document.getElementById("offcanvasNombreNuevo").style.display = "none";
                    }



                    if(botonEditar.textContent == "Cancelar"){
                        
                        // Restore original values
                        if(formEditar.originalValues){
                            await cargarOptionPuestosSegunDepartamento(parseInt(formEditar.originalValues.departamento), "offcanvasPuesto"); //LLenar selects con los puestos correspondientes|
                            formEditar.offcanvasNombre.value = formEditar.originalValues.nombre;
                            formEditar.offcanvasDepartamento.value = formEditar.originalValues.departamento;
                            setSelectValue('offcanvasPuesto', formEditar.originalValues.puesto);
                            formEditar.offcanvasJefeDirecto.value = formEditar.originalValues.jefeDirecto;
                            formEditar.offCanvasIdPadre.value = formEditar.originalValues.idPadre;
                            document.getElementById('offcanvasImgEmpleado').src = formEditar.originalValues.imgSrc;
                            if (document.getElementById('offcanvasNombreNuevo')) {
                                formEditar.offcanvasNombreNuevo.value = formEditar.originalValues.nombreNuevo;
                            }   
                            if(formEditar.originalValues.enlaceEstaOculto){
                                document.getElementById('infoAdicional').style.display = "none";
                            }
                        }

                        botonEditar.textContent = "Editar";
                        botonEditar.style.backgroundColor = "#2a9d8f";
                        document.getElementById("btnPopoverJefeDirecto").style.display = "none";
                        document.getElementById("btnEditarFotoPerfilEmpleado").style.display = "none";
                        document.getElementById("btnSubmitEmpleadoOffCanvas").style.display = "none";
                        document.getElementById("offcanvasPuestoNuevo").style.display = "none";


                        if (formEditar.originalValues.nombre == "") {
                            document.getElementById("offcanvasNombre").style.display = "none";
                        }


                        //Mostrar de nuevo opcion de sin jefe directo y seleccionarla
                        let optionSinJefe = document.getElementById('offcanvasJefeDirecto').querySelector('option[value="sinJefeDirecto"]');
                        if(!optionSinJefe){
                            let option = document.createElement("option");
                            option.text = "Sin jefe directo";
                            option.value = "sinJefeDirecto";  
                            document.getElementById('offcanvasJefeDirecto').appendChild(option);
                        }
                        formEditar.offcanvasJefeDirecto.value = "sinJefeDirecto";


                        //Regresamos el orden de los elementos a como estaban
                        const jefeDirecto = document.getElementById("offcanvasJefeDirecto").parentNode;
                        const descripcionPuesto = document.getElementById("offcanvasDescripcionPuesto").parentNode; // Asegúrate de usar el ID correcto
                    
                        // Insertar "Jefe Directo" después de "Descripción del puesto"
                        descripcionPuesto.parentNode.insertBefore(jefeDirecto, descripcionPuesto.nextElementSibling);
                    }
                }
            }
        }
        


        // Mostrar input si se elige "Nuevo Puesto"
        document.getElementById("offcanvasNombre").addEventListener("change", function () {
            let nuevoUsuarioInput = document.getElementById("offcanvasNombreNuevo");
            if (this.value === "") {
                nuevoUsuarioInput.style.display = "inline-block";   
            } else {
                nuevoUsuarioInput.style.display = "none";
                nuevoUsuarioInput.value = "";
            }
        });

        // Función para mostrar input si se elige "Nuevo Puesto"
        function setupNuevoPuestoListener() {
            let nuevoPuestoInput = document.getElementById("offcanvasPuestoNuevo");
            let puestoSelect = document.getElementById("offcanvasPuesto");

            if (puestoSelect.value === "") {
                nuevoPuestoInput.style.display = "inline-block";
            } else {
                nuevoPuestoInput.style.display = "none";
                nuevoPuestoInput.value = "";
            }
        }



        const btnEliminarDepartamento = document.getElementById('btnEliminarDepartamentoOffCanvas');
        if (btnEliminarDepartamento) {
            btnEliminarDepartamento.addEventListener('click', function () {
                let nodeId = parseInt(document.getElementById('offcanvasIdOrganigramaDepartamento').value);
                eliminarNodo(nodeId);
            });
        }

        const btnEliminarEmpleado = document.getElementById('btnEliminarEmpleadoOffCanvas');
        if (btnEliminarEmpleado) {
            btnEliminarEmpleado.addEventListener('click', function () {
                let nodeId = parseInt(document.getElementById('offCanvasIdOrganigramaEmpleado').value);
                eliminarNodo(nodeId);
            });
        }



        function eliminarNodo(nodeId) {
            overlay.style.display = "block";

            Swal.fire({
                title: "¿Eliminar este nodo?",
                text: "Se eliminará el nodo y sus hijos se asignarán al nodo más cercano. Esta acción no se puede deshacer.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Eliminar",
                cancelButtonText: "Cancelar",
                didOpen: () => {
                    // Mover el modal de SweetAlert2 al contenedor fijo
                    const swalModal = document.querySelector(".swal2-container");
                    const swalContainer = document.getElementById("swal-fixed-container");
                    if (swalModal && swalContainer) {
                        swalContainer.appendChild(swalModal);
                        swalModal.style.position = "absolute";
                        swalModal.style.top = "50%";
                        swalModal.style.left = "50%";
                        swalModal.style.transform = "translate(-50%, -50%)";
                        swalModal.style.width = "100%";

                    }

                    document.querySelector(".swal2-container").classList.add("swal2-backdrop-hide");
                    document.querySelector(".swal2-container").classList.remove("swal2-backdrop-show");

                    //SI ESTAMOS EN PANTALLA COMPLETA cambiar color de fondo de sweetalert y overlay
                    if (document.fullscreenElement) {
                        document.querySelector(".swal2-container").classList.add("swal2-backdrop-show");
                        document.querySelector(".swal2-container").classList.remove("swal2-backdrop-hide");
                    }

                },
            }).then((result) => {
                overlay.style.display = "none";
    
                if (result.isConfirmed) {

                    //Buscar el nodo padre
                    let nodo = mockData.find(n => n.id === nodeId);
                    let idPadre = nodo.idPadre;

                    if(idPadre == null){
                        messageAlert("Error", "No se puede eliminar solo la raíz del organigrama. Puedes eliminarlo eliminando toda la rama.", "error", "Aceptar")
                            .then(() => {
                                overlay.style.display = "none";
                                overlay.style.zIndex = "1060";
                            });
                        return;
                    }

                    var token = $('input[name="__RequestVerificationToken"]').val(); // Obtener el token CSRF

                    $.ajax({
                        type: "POST",
                        url: `/Organigrama?handler=EliminarNodo`,  // Ajusta según tu Razor Page
                        data: JSON.stringify(nodeId),
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader("XSRF-TOKEN", token); // Agregar token CSRF
                        },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json"
                    }).done(function (data) {
                        if (data.success) {
                            messageAlert("Eliminado", "El nodo ha sido eliminado exitosamente", "success", "Aceptar")
                                .then(() => {
                                    overlay.style.display = "none";
                                    overlay.style.zIndex = "1060";    
                                    //cerrar offcanvas
                                    const offcanvas = bootstrap.Offcanvas.getInstance(document.querySelector(".offcanvas.show"));
                                    offcanvas.hide();
                                    peticionDatosOrganigrama();
                                });
                        } else {
                            messageAlert("Error", "Hubo un problema al eliminar el nodo.", "error", "Aceptar")
                                .then(() => {
                                    overlay.style.display = "none";
                                    overlay.style.zIndex = "1060";
                                });
                        }
                    }
                    ).fail(function (xhr, status, error) {
                        messageAlert("Error", "Hubo un problema al eliminar el nodo.", "error", "Aceptar")
                            .then(() => {
                                overlay.style.display = "none";
                                overlay.style.zIndex = "1060";
                            });
                        console.error("Error en la petición:", status, error);
                    }
                    );



                }
            });

            
        }








        //-------------------------------------------------------Encargado del desplazamiento de la pagina al arrastrar el mouse
        let isDragging = false;
        let startX, startY;
        let translateX = 0, translateY = 0;
        let scale = 1;
        const minScale = 0.2, maxScale = 2;
        
        const treeContainer = document.getElementById("tree");
        const treeWrapper = document.querySelector(".contenedorArbol");

        // Verificar si hay un modal visible
        const isModalOpen = () => {
            return document.querySelector(".modal.show") !== null || document.querySelector(".swal2-container") !== null ;
        };

        // Verificar si el cursor está dentro de un offcanvas abierto
        const isCursorInOffcanvas = (e) => {
            const offcanvas = document.querySelector(".offcanvas.show");
            if (!offcanvas) return false;
            const rect = offcanvas.getBoundingClientRect();
            return e.clientX >= rect.left && e.clientX <= rect.right && e.clientY >= rect.top && e.clientY <= rect.bottom;
        };

        
        // Aplicar transformaciones
        const updateTransform = () => {
            treeContainer.style.transform = `translate(${translateX}px, ${translateY}px) scale(${scale})`;
        };
        
        // Detectar inicio del arrastre
        treeWrapper.addEventListener("mousedown", (e) => {
            if (isModalOpen() || isCursorInOffcanvas(e)) return; // No hacer nada si hay un modal o el cursor está en el offcanvas
            isDragging = true;
            startX = e.clientX - translateX;
            startY = e.clientY - translateY;
            treeContainer.style.cursor = "grabbing";
        });
        
        // Mover el organigrama
        treeWrapper.addEventListener("mousemove", (e) => {
            if (isModalOpen() || isCursorInOffcanvas(e) || !isDragging) return; // No mover si hay un modal, el cursor está en el offcanvas o no se está arrastrando            e.preventDefault();
        
            translateX = e.clientX - startX;
            translateY = e.clientY - startY;
        
            updateTransform();
        });
        
        // Soltar el arrastre
        treeWrapper.addEventListener("mouseup", () => {
            isDragging = false;
            treeContainer.style.cursor = "grab";
        });
        
        treeWrapper.addEventListener("mouseleave", () => {
            isDragging = false;
            treeContainer.style.cursor = "grab";
        });
        
        // Zoom con la rueda del mouse
        treeWrapper.addEventListener("wheel", (e) => {
            if (isModalOpen() || isCursorInOffcanvas(e)) return; // No hacer nada si hay un modal o el cursor está en el offcanvas
            e.preventDefault();
        
            const scaleAmount = 0.1;
            const prevScale = scale;
        
            // Calcular la nueva escala
            scale = Math.max(minScale, Math.min(maxScale, scale + (e.deltaY < 0 ? scaleAmount : -scaleAmount)));
        
            // Ajustar la posición para mantener el zoom centrado en el cursor
            const rect = treeContainer.getBoundingClientRect();
            const offsetX = (e.clientX - rect.left) / rect.width;
            const offsetY = (e.clientY - rect.top) / rect.height;
        
            translateX -= (e.clientX - startX) * (scale - prevScale);
            translateY -= (e.clientY - startY) * (scale - prevScale);
        
            updateTransform();  
        });
        
        const fitAndCenterOrganigrama = () => {
            requestAnimationFrame(() => {
                const wrapperBounds = treeWrapper.getBoundingClientRect();
                const treeBounds = treeContainer.getBoundingClientRect(); //Haciendo que el tamaño de el organigrama siempre se considere el mismo para el fit

                //reducir el treeBounds.width si ya no es 1 la scala
                treeBounds.width = treeBounds.width / scale;
                treeBounds.height = treeBounds.height / scale;
        
                // Calcular escala óptima manteniendo proporciones
                const scaleX = wrapperBounds.width / treeBounds.width;
                const scaleY = wrapperBounds.height / treeBounds.height;
                const newScale = Math.min(scaleX, scaleY) * 0.9;
        
                // Aplicar restricciones de escala
                scale = Math.max(minScale, Math.min(maxScale, newScale));
        
                // 🔹 CORRECCIÓN: Ajuste del centrado
                translateX = (wrapperBounds.width - (treeBounds.width * scale)) / 2;
                translateY = (wrapperBounds.height - (treeBounds.height * scale)) / 2;
        
                // Aplicar transformación
                updateTransform();
                
            });
        };
        
        // Ejecutar después de la carga
        window.addEventListener("load", () => {
            setTimeout(() => {

                fitAndCenterOrganigrama();
            }, 500);
        });
        
        // Ajustar en caso de redimensionar
        window.addEventListener("resize", fitAndCenterOrganigrama);
        


        // ------------------------------------------------------------------------------------Funciones para pantalla completa


        const openFullscreen = () => {
            const elem = document.querySelector(".contenedorArbol"); // Elemento a poner en pantalla completa
        
            if (elem.requestFullscreen) {
                elem.requestFullscreen();
            } else if (elem.mozRequestFullScreen) { // Firefox
                elem.mozRequestFullScreen();
            } else if (elem.webkitRequestFullscreen) { // Chrome, Safari y Opera
                elem.webkitRequestFullscreen();
            } else if (elem.msRequestFullscreen) { // IE/Edge
                elem.msRequestFullscreen();
            }
        };
        
        const closeFullscreen = () => {
            if (document.exitFullscreen) {
                document.exitFullscreen();
            } else if (document.mozCancelFullScreen) { // Firefox
                document.mozCancelFullScreen();
            } else if (document.webkitExitFullscreen) { // Chrome, Safari y Opera
                document.webkitExitFullscreen();
            } else if (document.msExitFullscreen) { // IE/Edge
                document.msExitFullscreen();
            }
        };
        
        // Botón para alternar entre pantalla completa y normal
        const toggleFullscreen = () => {
            if (!document.fullscreenElement) {
                openFullscreen();
                fitAndCenterOrganigrama();

            } else {
                closeFullscreen();
                fitAndCenterOrganigrama();
            }
        };




        // ------------------------------------------------------------------------------------PUESTOS Y DEPARTAMENTOS

        // Cargar departamentos y puestos en modal de lectura
        document.getElementById("PuestosDepartamentosModal").addEventListener("show.bs.modal", cargarDepartamentosPuestos);

        //Cargar departamentos en el modal de edicion de puestos
        document.getElementById("agregarEditarPuestos").addEventListener("show.bs.modal", cargarOptionDepartamentosSelect("departamentoPuesto"));


        async function obtenerListaDepartamentos(){
            const response = fetch('/Organigrama?handler=Departamentos')
            .then(response => response.json())
            .then(data => {
                return data.departamentos;
            })
            .catch(error => {
                console.error("Error al cargar los departamentos:", error);
            });
            return response;
        }

        async function obtenerListaPuestos(){
            const response = fetch('/Organigrama?handler=Puestos')
            .then(response => response.json())
            .then(data => {
                return data.puestos;
            })
            .catch(error => {
                console.error("Error al cargar los puestos:", error);
            });
            return response;
        }

        async function obtenerListaUsuarios(){
            const response = fetch('/Organigrama?handler=Usuarios')
            .then(response => response.json())
            .then(data => {
                return data.usuarios;
            })
            .catch(error => {
                console.error("Error al cargar los usuarios:", error);
            });
            return response;
        }


        async function cargarOptionDepartamentosSelect(idSelect) {
            try {
                const departamentos = await obtenerListaDepartamentos();

                const selectDepartamentos = document.getElementById(idSelect);
                selectDepartamentos.innerHTML = `
                    <option value="">-- Nuevo Departamento --</option>
                    ${departamentos.map(departamento => `
                        <option value="${departamento.id}">${departamento.nombre}</option>
                    `).join('')}
                `;
            } catch (error) {
                console.error("Error al cargar los departamentos:", error);
            }
        }

        async function cargarOptionUsuariosSelect(idSelect) {
            try {
                const usuarios = await obtenerListaUsuarios();

                const selectUsuarios = document.getElementById(idSelect);
                selectUsuarios.innerHTML = `
                    <option value="">-- Nuevo Usuario --</option>
                    <option value="0">-- Vacante --</option>
                    ${usuarios.map(usuario => `
                        <option value="${usuario.id}">${usuario.nombre}</option>
                    `).join('')}
                `;
            } catch (error) {
                console.error("Error al cargar los usuarios:", error);
            }
        }


        async function cargarOptionPuestosAgrupadosPorDepartamentoSelect(idSelect) {
            try {
                const departamentos = await obtenerListaDepartamentos();
                const puestos = await obtenerListaPuestos();

                const selectPuestos = document.getElementById(idSelect);
                selectPuestos.innerHTML = `
                    <option value="">-- Nuevo Puesto --</option>
                    ${departamentos.map(departamento => `
                        <optgroup label="${departamento.nombre}">
                            ${puestos.filter(puesto => puesto.idDepartamento === departamento.id).map(puesto => `
                                <option value="${puesto.id}">${puesto.nombre}</option>
                            `).join('')}
                        </optgroup>
                    `).join('')}
                `;
            } catch (error) {
                console.error("Error al cargar los puestos:", error);
            }
        }

        async function seleccionDepartamentoSegunJefe() {
            var jefeSelect = document.getElementById('offcanvasJefeDirecto');
            let selectedOption = jefeSelect.options[jefeSelect.selectedIndex]; // Obtener la opción seleccionada

            let idDepartamento = selectedOption.getAttribute("data-idDepartamento");

            console.log("El id es: ", idDepartamento);

            document.getElementById('offcanvasDepartamento').value = idDepartamento;

            await cargarOptionPuestosSegunDepartamento(idDepartamento, "offcanvasPuesto");

            setupNuevoPuestoListener()
        }


        async function cargarOptionPuestosSegunDepartamento(idDepartamento, idSelect) {
            try {
                const puestos = await obtenerListaPuestos();

                const selectPuestos = document.getElementById(idSelect);
                selectPuestos.innerHTML = `
                    <option value="">-- Nuevo Puesto --</option>
                    ${puestos.filter(puesto => puesto.idDepartamento == idDepartamento).map(puesto => `
                        <option value="${puesto.id}">${puesto.nombre}</option>
                    `).join('')}
                `;
            } catch (error) {
                console.error("Error al cargar los puestos:", error);
            }
        }


        async function cargarDepartamentosPuestos() {
            const contenedor = document.getElementById("contenedorDepartamentos");
            contenedor.innerHTML = '<div class="alert alert-info text-center">Cargando departamentos...</div>';
        
            try {

                const departamentos = await obtenerListaDepartamentos();
                const puestos = await obtenerListaPuestos();


                // Si no hay departamentos
                if (departamentos.length === 0) {
                    contenedor.innerHTML = '<div class="alert alert-info text-center">No hay departamentos registrados</div>';
                    return;
                }


                let html = "";
                departamentos.forEach(departamento => {
                    // Filtrar los puestos que pertenecen a este departamento
                    const puestosPorDepartamento = puestos.filter(puesto => puesto.idDepartamento === departamento.id);

                    html += `
                        <div class="accordion-item">
                            <h2 class="accordion-header" id="heading-${departamento.id}">
                                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapse-${departamento.id}" aria-expanded="false" aria-controls="collapse-${departamento.id}">
                                    ${departamento.nombre}
                                </button>
                                <div class="contenedorBotones">
                                    ${departamento.id !== 2 && departamento.id !== 3 ? `
                                        <button class="btn buttonGeneral botonEliminar" onclick="eliminarDepartamento(${departamento.id}, ${puestosPorDepartamento.length})">
                                            <span class="material-symbols-outlined">delete</span>
                                        </button>` : ""}
                                    <button class="btn buttonGeneral botonEditar" data-bs-toggle="modal" data-bs-target="#agregarEditarDepartamentos" 
                                        onclick="editarDepartamento(this)" data-id="${departamento.id}" data-nombre="${departamento.nombre}" >                                       
                                        <span class="material-symbols-outlined">edit</span>
                                    </button>
                                </div>
                            </h2>
                            <div id="collapse-${departamento.id}" class="accordion-collapse collapse" aria-labelledby="heading-${departamento.id}">
                                <div class="accordion-body">
                                    <h5>Puestos</h5>
                                    ${puestosPorDepartamento.length === 0 ? '<div class="alert alert-info text-center">No hay puestos registrados en este departamento</div>' : ""}
                                    <ul class="puesto-list">
                                        ${puestosPorDepartamento.map(puesto =>  `
                                            <li class="puesto-item">
                                                <div class="contenedorPuesto">
                                                    <p>
                                                        ${puesto.id !== 3 && puesto.id !== 5 ? `<span class="material-symbols-outlined">east</span>` 
                                                        : `<span class="material-symbols-outlined" style="color: rgb(183, 183, 0);">star</span>`}
                                                        ${puesto.nombre}
                                                    </p>
                                                    <div class="contenedorBotones">
                                                        ${puesto.id !== 3 && puesto.id !== 5 ? `
                                                            <button class="btn buttonGeneral botonEliminar" onclick="eliminarPuesto(${puesto.id})">
                                                                <span class="material-symbols-outlined">delete</span>
                                                            </button>` : ""}
                                                        <button class="btn buttonGeneral botonEditar" data-bs-toggle="modal" data-bs-target="#agregarEditarPuestos"
                                                            onclick="editarPuesto(this)" data-id="${puesto.id}" data-nombre="${puesto.nombre}" data-departamento="${puesto.idDepartamento}" data-descripcion="${puesto.descripcionPuesto || ''}" data-url="${puesto.descripcionPuestoDocumento?.url || ''}" data-idDocumento="${puesto.descripcionPuestoDocumento?.id || ''}">
                                                            <span class="material-symbols-outlined">edit</span>
                                                        </button>
                                                    </div>
                                                </div>
                                            </li>   
                                        `).join('')}
                                    </ul>
                                </div>
                            </div>
                        </div>
                    `;
                });
        
                contenedor.innerHTML = html;
            } catch (error) {
                console.error("Error al cargar los departamentos:", error);
                contenedor.innerHTML = '<div class="alert alert-danger text-center">Error al cargar los departamentos</div>';
            }
        }
        
        


        // Función para manejar la eliminación del puesto
        function eliminarPuesto(idPuesto) {
            overlay.style.display = "block";
            overlay.style.zIndex = "12000";

            Swal.fire({
                title: "¿Estás seguro de eliminar el puesto?",
                text: "Esta acción no se puede deshacer.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminar",
                cancelButtonText: "Cancelar",
                didOpen: () => {
                    // Mover el modal de SweetAlert2 al contenedor fijo
                    const swalModal = document.querySelector(".swal2-container");
                    const swalContainer = document.getElementById("swal-fixed-container");
                    if (swalModal && swalContainer) {
                        swalModal.classList.add("swal2-superior");
                        swalContainer.appendChild(swalModal);
                        swalModal.style.position = "absolute";
                        swalModal.style.top = "50%";
                        swalModal.style.left = "50%";
                        swalModal.style.transform = "translate(-50%, -50%)";
                        swalModal.style.width = "100%";

                    }

                    document.querySelector(".swal2-container").classList.add("swal2-backdrop-hide");
                    document.querySelector(".swal2-container").classList.remove("swal2-backdrop-show");

                    //SI ESTAMOS EN PANTALLA COMPLETA cambiar color de fondo de sweetalert y overlay
                    if (document.fullscreenElement) {
                        document.querySelector(".swal2-container").classList.add("swal2-backdrop-show");
                        document.querySelector(".swal2-container").classList.remove("swal2-backdrop-hide");
                    }

                },
            }).then((result) => {
                overlay.style.display = "none";
                overlay.style.zIndex = "1060";

                if (result.isConfirmed) {

                
                //Comprobar si el puesto esta usado en el orgnigrama, si lo tiene algun nodo no podemos borrarlo
                let nodo = mockData.find(n => n.idPuesto == idPuesto);

                if(nodo){
                    messageAlert("Error", "No se puede eliminar el puesto porque está en uso en el organigrama.", "error", "Aceptar")
                        .then(() => {   
                            overlay.style.display = "none";
                            overlay.style.zIndex = "1060";
                        });
                    return;
                }


                var token = $('input[name="__RequestVerificationToken"]').val(); // Obtener el token CSRF

                // Crear un nuevo FormData
                var info = new FormData();
                info.append("Id", idPuesto);
                info.append("eliminarCardex", false);
    

                    //Comprobar si hay kardex con el puesto para indicar que se daran de baja temporal si se elimina
                    $.ajax({
                        type: "POST",
                        url: `/Organigrama?handler=NumeroCardexPuesto`,  // Ajusta según tu Razor Page
                        data: info,
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader("XSRF-TOKEN", token); // Agregar token CSRF
                        },
                        processData: false,
                        contentType: false,
                        dataType: "json"
                    }).done(function (data) {
                        if (data.success) {
                            if (data.numeroCardexPuesto > 0) {
                                overlay.style.display = "block";
                                overlay.style.zIndex = "12000";
                    
                                // Mostrar mensaje de advertencia
                                Swal.fire({
                                    title: "Advertencia",
                                    text: `El puesto tiene ${data.numeroCardexPuesto} Cardex asignados. Si eliminas el puesto, todos los usuarios que cuenten con él se darán de baja temporalmente.`,
                                    icon: "warning",
                                    showCancelButton: true,
                                    cancelButtonText: "Cancelar",
                                    confirmButtonText: "Sí, eliminar",
                                    reverseButtons: true,
                                    didOpen: () => {
                                        // Mover el modal de SweetAlert2 al contenedor fijo
                                        const swalModal = document.querySelector(".swal2-container");
                                        const swalContainer = document.getElementById("swal-fixed-container");
                                        if (swalModal && swalContainer) {
                                            swalModal.classList.add("swal2-superior");
                                            swalContainer.appendChild(swalModal);
                                            swalModal.style.position = "absolute";
                                            swalModal.style.top = "50%";
                                            swalModal.style.left = "50%";
                                            swalModal.style.transform = "translate(-50%, -50%)";
                                            swalModal.style.width = "100%";
                                        }
                                        document.querySelector(".swal2-container").classList.add("swal2-backdrop-hide");
                                        document.querySelector(".swal2-container").classList.remove("swal2-backdrop-show");
                                        //SI ESTAMOS EN PANTALLA COMPLETA cambiar color de fondo de sweetalert y overlay
                                        if (document.fullscreenElement) {
                                            document.querySelector(".swal2-container").classList.add("swal2-backdrop-show");
                                            document.querySelector(".swal2-container").classList.remove("swal2-backdrop-hide");
                                        }
                                    },
                                }).then((result) => {
                                    overlay.style.display = "none";
                                    overlay.style.zIndex = "1060";

                                    if (!result.isConfirmed) {
                                        // Si el usuario no confirma, nosalimos de toda la funcion
                                        return;
                                        
                                    } else {
                                        // Cambiar el valor a true si se confirma la eliminación
                                        info.set("eliminarCardex", true);

                                        peticionEliminarPuesto(info, token);
                                    }

                                });
                                
                            } else {
                                peticionEliminarPuesto(info, token);
                            }


                        } else {
                            messageAlert("Error", "Hubo un problema al comprobar el Cardex.", "error", "Aceptar")
                                .then(() => {
                                    overlay.style.display = "none";
                                    overlay.style.zIndex = "1060";
                                });
                            return;
                        }
                    }).fail(function (xhr, status, error) {
                        messageAlert("Error", "Hubo un problema al comprobar el Cardex", "error", "Aceptar")
                            .then(() => {
                                overlay.style.display = "none";
                                overlay.style.zIndex = "1060";
                            }
                        );
                        console.error("Error en la petición:", status, error);
                        return;
                    });




                }
                
            });
        }


        function peticionEliminarPuesto(info, token){
            // Si el usuario confirma la eliminación, proceder a eliminar el puesto
            $.ajax({
                type: "POST",
                url: `/Organigrama?handler=EliminarPuesto`,  // Ajusta según tu Razor Page
                data: info,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN", token); // Agregar token CSRF
                },
                processData: false,
                contentType: false,
                dataType: "json"
            }).done(function (data) {

                if(data.success){
                    messageAlert("Eliminado", "El puesto ha sido eliminado exitosamente", "success", "Aceptar")
                        .then(() => {
                            overlay.style.display = "none";
                            overlay.style.zIndex = "1060";    
                            cargarDepartamentosPuestos();
                        });
                } else {
                    messageAlert("Error", "Hubo un problema al eliminar el puesto.", "error", "Aceptar")
                        .then(() => {
                            overlay.style.display = "none";
                            overlay.style.zIndex = "1060";
                        });
                }

            }).fail(function (xhr, status, error) {
                messageAlert("Error", "Hubo un problema al eliminar el puesto.", "error", "Aceptar")
                    .then(() => {
                        overlay.style.display = "none";
                        overlay.style.zIndex = "1060";
                    });
                console.error("Error en la petición:", status, error);
            });
        }




        function eliminarDepartamento(idDepartamento, numeroPuestosDepartamento) {
            overlay.style.display = "block";
            overlay.style.zIndex = "12000";
            Swal.fire({
                title: "¿Estás seguro de eliminar el departamento?",
                text: "Esta acción no se puede deshacer.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminar",
                cancelButtonText: "Cancelar",
                didOpen: () => {
                    // Mover el modal de SweetAlert2 al contenedor fijo
                    const swalModal = document.querySelector(".swal2-container");
                    const swalContainer = document.getElementById("swal-fixed-container");
                    if (swalModal && swalContainer) {
                        //agregar clse al modal
                        swalModal.classList.add("swal2-superior");
                        swalContainer.appendChild(swalModal);
                        swalModal.style.position = "absolute";
                        swalModal.style.top = "50%";
                        swalModal.style.left = "50%";
                        swalModal.style.transform = "translate(-50%, -50%)";
                        swalModal.style.width = "100%";

                    }

                    document.querySelector(".swal2-container").classList.add("swal2-backdrop-hide");
                    document.querySelector(".swal2-container").classList.remove("swal2-backdrop-show");

                    //SI ESTAMOS EN PANTALLA COMPLETA cambiar color de fondo de sweetalert y overlay
                    if (document.fullscreenElement) {
                        document.querySelector(".swal2-container").classList.add("swal2-backdrop-show");
                        document.querySelector(".swal2-container").classList.remove("swal2-backdrop-hide");
                    }

                },
            }).then((result) => {
                overlay.style.display = "none";
                overlay.style.zIndex = "1060";

                if (result.isConfirmed) {

                    //Comprobar si el puesto esta usado en el orgnigrama, si lo tiene algun nodo no podemos borrarlo
                    let nodo = mockData.find(n => n.idDepartamento == idDepartamento);
                    

                    if (numeroPuestosDepartamento > 0) {
                        messageAlert("Error", "No se puede eliminar un departamento con puestos asignados.", "error", "Aceptar")
                        .then(() => {
                            overlay.style.display = "none";
                            overlay.style.zIndex = "1060";
                        });
                        return;
                    }

                    if(nodo){
                        messageAlert("Error", "No se puede eliminar el puesto porque está en uso en el organigrama.", "error", "Aceptar")
                            .then(() => {   
                                overlay.style.display = "none";
                                overlay.style.zIndex = "1060";
                            });
                        return;
                    }



                        var token = $('input[name="__RequestVerificationToken"]').val(); // Obtener el token CSRF

                        var data = {
                            Id: idDepartamento,
                        };
        

                        $.ajax({
                            type: "POST",
                            url: `/Organigrama?handler=EliminarDepartamento`,  // Ajusta según tu Razor Page
                            data: JSON.stringify(data),
                            beforeSend: function (xhr) {
                                xhr.setRequestHeader("XSRF-TOKEN", token); // Agregar token CSRF
                            },
                            contentType: "application/json; charset=utf-8",
                            dataType: "json"
                        }).done(function (data) {

                            if(data.success){
                            
                                messageAlert("Eliminado", "El departamento ha sido eliminado", "success", "Aceptar")
                                    .then(() => {
                                        overlay.style.display = "none";
                                        overlay.style.zIndex = "1060";    
                                        cargarDepartamentosPuestos();
                                    });
                            } else {
                                messageAlert("Error", "Hubo un problema al eliminar el departamento.", "error", "Aceptar")
                                    .then(() => {
                                        overlay.style.display = "none";
                                        overlay.style.zIndex = "1060";
                                    });
                            }
        
                        }).fail(function (xhr, status, error) {
                            messageAlert("Error", "Hubo un problema al eliminar el departamento.", "error", "Aceptar")
                                .then(() => {
                                    overlay.style.display = "none";
                                    overlay.style.zIndex = "1060";
                                });
                            console.error("Error en la petición:", status, error);
                        });
                    
                    
                }
            });
        }


        function editarPuesto(button) {
            // Obtener los valores del botón
            let id = button.getAttribute("data-id");
            let nombre = button.getAttribute("data-nombre");
            let departamento = button.getAttribute("data-departamento");
            let descripcion = button.getAttribute("data-descripcion");
            let idDocumento = button.getAttribute("data-idDocumento") || 0;
            let url = button.getAttribute("data-url");

            //cambiar texto boton enviar fomruario atraves de su tipo que es submit
            document.getElementById("btnEnviarModalPuesto").textContent = "Editar";

            //cambiar el action del formulario
            document.getElementById("formPuesto").action = "/Organigrama?handler=EditarPuesto";
            
            if(id == 3 || id == 5){
                document.getElementById("departamentoPuesto").disabled = true;
            } else {
                document.getElementById("departamentoPuesto").disabled = false;
            }
        
            // Llenar los campos del modal con la información
            document.getElementById("idPuesto").value = id;
            document.getElementById("nombrePuesto").value = nombre;
            document.getElementById("departamentoPuesto").value = departamento;
            document.getElementById("descripcionPuesto").textContent = descripcion || ""; // Si está vacío, evita mostrar "null"
            document.getElementById("documentoDescripcionPuesto_Id").value = idDocumento;

            if(url && url !== "") 
            {
                document.getElementById("contenedorDocumentoDescripcionPuesto").style.setProperty("display", "flex", "important");
                document.getElementById("documentoActualDescripcionPuesto").href = url;
            }
            else {
                document.getElementById("contenedorDocumentoDescripcionPuesto").style.setProperty("display", "none", "important");
            }
        }


        //Actualizar input de documentos si cambio.
        document.getElementById("documentoDescripcionPuesto").addEventListener("change", (event) => {
            const input = event.target;
            const file = input.files[0];

            if (file) {
                const id = input.id; //El input poculto tinee el mismo id actual per agregado _Id
                const inputOculto = document.getElementById(`${id}_Id`); // Obtener el input oculto para cambiarlo a negativo si se edito
                if (inputOculto) {
                    inputOculto.value *= -1;
                }
            }
        });


        //CAMBIOAR EL INPUT OCULTO PARA SABER SI UN DOCUMENTO FUE ELIMINADO
        function cambiarEstadoEliminado(event) {
            // Busca el botón más cercano con la clase "botonEliminarDocumento"
            const button = event.target.closest(".botonEliminarDocumento");
            if (button) {
                const span = button.querySelector("span");
                // Busca el enlace más cercano en el mismo contenedor que el botón
                const etiqueta = button.parentElement.querySelector("a");
                const input = button.parentElement.querySelector("input");
                if (span && etiqueta) {
                    // Alterna el texto dentro del span
                    span.textContent = span.textContent === "delete" ? "undo" : "delete";
                    // Alterna el estilo de texto tachado en la etiqueta
                    etiqueta.style.textDecoration = etiqueta.style.textDecoration === "line-through" ? "none" : "line-through";
                    //ELterna el value del input
                    input.value = input.value === "true" ? "false" : "true";
                }
            }
        }






        function editarDepartamento(button){
            // Obtener los valores del botón
            let id = button.getAttribute("data-id");
            let nombre = button.getAttribute("data-nombre");

            //cambiar texto boton enviar fomruario atraves de su tipo que es submit
            document.getElementById("btnEnviarModalDepartamento").textContent = "Editar";

            //cambiar el action del formulario
            document.getElementById("formDepartamento").action = "/Organigrama?handler=EditarDepartamento";
        
            // Llenar los campos del modal con la información
            document.getElementById("idDepartamento").value = id;
            document.getElementById("nombreDepartamento").value = nombre;
        }
        

        function agregarPuesto(button){

            //cambiar el action del formulario
            document.getElementById("formPuesto").action = "/Organigrama?handler=AgregarPuesto";

            //cambiar texto boton enviar fomruario atraves de su tipo que es submit
            document.getElementById("btnEnviarModalPuesto").textContent = "Guardar";

            document.getElementById("departamentoPuesto").disabled = false;

            //limpiar los campos
            document.getElementById("idPuesto").value = "";
            document.getElementById("nombrePuesto").value = "";
            document.getElementById("departamentoPuesto").value = "";

            //cambiar texto de nuevo departamento a seleecione una opcion
            document.getElementById("departamentoPuesto").options[0].text = "Seleccione una opción";


            document.getElementById("descripcionPuesto").value = "";
            document.getElementById("documentoDescripcionPuesto").value = "";
            document.getElementById("contenedorDocumentoDescripcionPuesto").style.setProperty("display", "none", "important");

        }

        function agregarDepartamento(button){

            //cambiar el action del formulario
            document.getElementById("formDepartamento").action = "/Organigrama?handler=AgregarDepartamento";

            //cambiar texto boton enviar fomruario atraves de su tipo que es submit
            document.getElementById("btnEnviarModalDepartamento").textContent = "Guardar";

            //limpiar los campos
            document.getElementById("idDepartamento").value = "";
            document.getElementById("nombreDepartamento").value = "";
        }


        //Departamento GUARDAR
        $(document).ready(function () {
            $("#formDepartamento").on("submit", function (e) {
                e.preventDefault(); // Evita el envío tradicional

                var token = $('input[name="__RequestVerificationToken"]').val(); // Obtener el token CSRF

                var data = {
                    Id: $("#idDepartamento").val() || 0,
                    Nombre: $("#nombreDepartamento").val()
                };

                var controlador = ($("#btnEnviarModalDepartamento").text() == "Guardar") ? "GuardarDepartamento" : "EditarDepartamento";

                $.ajax({
                    type: "POST",
                    url: `/Organigrama?handler=${controlador}`,  // Ajusta según tu Razor Page
                    data: JSON.stringify(data),
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN", token); // Agregar token CSRF
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json"
                }).done(function (data) {

                    if(data.success){
                        messageAlert("Guardado", "La información del departamento ha sido guardada.", "success", "Aceptar")
                            .then(() => {
                                overlay.style.display = "none";
                                overlay.style.zIndex = "1060";    
                                cargarDepartamentosPuestos();
                                $('#agregarEditarDepartamentos').modal('hide');
                            });
                    }
                    else
                    {
                        messageAlert("Error", "Hubo un problema al guardar la información del departamento.", "error", "Aceptar")
                            .then(() => {
                                overlay.style.display = "none";
                                overlay.style.zIndex = "1060";
                            }
                        );
                    }

                }).fail(function (xhr, status, error) {
                    messageAlert("Error", "Hubo un problema al guardar la información del departamento.", "error", "Aceptar")
                        .then(() => {
                            overlay.style.display = "none";
                            overlay.style.zIndex = "1060";
                        });
                    console.error("Error en la petición:", status, error);
                });

                return false;
            });
        });


        $(document).ready(function () {
            $("#formPuesto").on("submit", function (e) {
                e.preventDefault(); // Evita el envío tradicional
        
                var token = $('input[name="__RequestVerificationToken"]').val(); // Obtener el token CSRF
        
                // Crear un nuevo FormData
                var formData = new FormData();
                formData.append("Id", $("#idPuesto").val());
                formData.append("Nombre", $("#nombrePuesto").val());
                formData.append("IdDepartamento", $("#departamentoPuesto").val());
                formData.append("DescripcionPuesto", $("#descripcionPuesto").val());
        
                // Obtener el archivo seleccionado
                var fileInput = $("#documentoDescripcionPuesto")[0];  // Asumiendo que el id del input file es "inputFile"
                if (fileInput.files.length > 0) {
                    formData.append("DescripcionPuestoDocumento.Archivo", fileInput.files[0]); // Nombre de la propiedad en el modelo
                }
        
                // Los demás datos
                formData.append("DescripcionPuestoDocumento.Id", $("#documentoDescripcionPuesto_Id").val());
                formData.append("DescripcionPuestoDocumento.IdTipoDocumento", 37); // Suponiendo un valor para IdTipoDocumento
                formData.append("DescripcionPuestoDocumento.IdUsuario", null); // Suponiendo un valor para IdUsuario
                formData.append("DescripcionPuestoDocumento.Eliminado", $("#documentoDescripcionPuestoEliminado").val()); // Suponiendo que el documento no está eliminado

                console.log("HOla", $("#documentoDescripcionPuestoEliminado"));

                var controlador = ($("#btnEnviarModalPuesto").text() == "Guardar") ? "GuardarPuesto" : "EditarPuesto";
 
        
                $.ajax({
                    type: "POST",
                    url: `/Organigrama?handler=${controlador}`,
                    data: formData,
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN", token); // Agregar token CSRF
                    },
                    processData: false, // No procesar los datos
                    contentType: false, // No establecer el contentType, porque FormData se encarga de eso
                    dataType: "json"
                }).done(function (data) {

                    if(data.success){
                        messageAlert("Guardado", "La información del puesto ha sido guardada.", "success", "Aceptar")
                            .then(() => {
                                overlay.style.display = "none";
                                overlay.style.zIndex = "1060";    
                                cargarDepartamentosPuestos();
                                $('#agregarEditarPuestos').modal('hide');
                            });
                    }
                    else
                    {
                        messageAlert("Error", "Hubo un problema al guardar la información del puesto.", "error", "Aceptar")
                            .then(() => {
                                overlay.style.display = "none";
                                overlay.style.zIndex = "1060";
                            }
                        );
                    }

                }).fail(function (xhr, status, error) {
                    messageAlert("Error", "Hubo un problema al guardar la información del puesto.", "error", "Aceptar")
                     .then(() => {
                        overlay.style.display = "none";
                        overlay.style.zIndex = "1060";
                    });
                    console.error("Error en la petición:", status, error);
                });
        
                return false;
            });
        });

        $(document).ready(function () {
            $("#btnSubmitEmpleadoOffCanvas").on("click", function (e) {
                e.preventDefault(); // Evita el envío tradicional

        
                // Validaciones condicionales
                if (!$("#offcanvasNombre").val() && !$("#offcanvasNombreNuevo").val()) {
                    messageAlert("Error", "Si no seleccionas un usuario, debes escribir un nombre.", "error", "Aceptar")
                        .then(() => {
                            overlay.style.display = "none";
                            overlay.style.zIndex = "1060";
                        }
                    );

                    return false;
                }
        
                if (!$("#offcanvasPuesto").val() && !$("#offcanvasPuestoNuevo").val()) {
                    messageAlert("Error", "Debes seleccionar un puesto o escribir un nuevo puesto.", "error", "Aceptar")
                        .then(() => {
                            overlay.style.display = "none";
                            overlay.style.zIndex = "1060";
                        }
                    );

                    return false;
                }
        
                var token = $('input[name="__RequestVerificationToken"]').val(); // Obtener el token CSRF
        
                var formData = new FormData();
                formData.append("Id", $("#offCanvasIdOrganigramaEmpleado").val());
                formData.append("IdUsuario", $("#offcanvasNombre").val() || null);
                formData.append("Nombre", $("#offcanvasNombreNuevo").val() || "");
                formData.append("IdPadre", $("#offcanvasJefeDirecto").val());
                formData.append("IdDepartamento.Id", $("#offcanvasDepartamento").val());
                formData.append("IdPuesto.Id", $("#offcanvasPuesto").val());
                formData.append("IdPuesto.Nombre", $("#offcanvasPuestoNuevo").val());
        
                // Obtener el archivo seleccionado
                var fileInput = $("#documentoFotoPerfilEmpleado")[0];
                if (fileInput.files.length > 0) {  
                    formData.append("IdDocumento.Archivo", fileInput.files[0]);
                }
        
                formData.append("IdDocumento.Id", $("#documentoFotoPerfilEmpleado_Id").val());
                formData.append("IdDocumento.IdTipoDocumento", 36);
        
                if (formEditar.originalValues.idPadre == $("#offcanvasJefeDirecto").val()) {
                    formData.append("jefeActualizado", false);
                } else {
                    formData.append("jefeActualizado", true);
                }
        
                // Mostrar datos del formData en la consola (debug)
                for (var pair of formData.entries()) {
                    console.log(pair[0] + ', ' + pair[1]);
                }
        
                $.ajax({
                    type: "POST",
                    url: `/Organigrama?handler=EditarNodoEmpleado`,  // Ajusta según tu Razor Page
                    data: formData,
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN", token); // Agregar token CSRF
                    },
                    processData: false,
                    contentType: false,
                    dataType: "json"
                }).done(function (data) {
                    if (data.success) {
                        messageAlert("Guardado", "La información del empleado ha sido guardada.", "success", "Aceptar")
                            .then(() => {
                                overlay.style.display = "none";
                                overlay.style.zIndex = "1060";
                                peticionDatosOrganigrama();
                                $('#nodeModal').offcanvas('hide');
                            });
                    } else {
                        messageAlert("Error", "Hubo un problema al guardar la información del empleado.", "error", "Aceptar")
                            .then(() => {
                                overlay.style.display = "none";
                                overlay.style.zIndex = "1060";
                            });
                    }
                }).fail(function (xhr, status, error) {
                    messageAlert("Error", "Hubo un problema al guardar la información del empleado.", "error", "Aceptar")
                        .then(() => {
                            overlay.style.display = "none";
                            overlay.style.zIndex = "1060";
                        });
                    console.error("Error en la petición:", status, error);
                });
        
                return false;
            });
        });
        

        //Actualizar input de documentos si cambio.
        document.getElementById("documentoFotoPerfilEmpleado").addEventListener("change", (event) => {
            const input = event.target;
            const file = input.files[0];

            if (file) {
                const id = input.id; //El input poculto tinee el mismo id actual per agregado _Id
                const inputOculto = document.getElementById(`${id}_Id`); // Obtener el input oculto para cambiarlo a negativo si se edito
                if (inputOculto) {
                    inputOculto.value *= -1;
                }
            }
        });



        $(document).ready(function () {
            $("#btnSubmitDepartamentoOffCanvas").on("click", function (e) {
                e.preventDefault(); // Evita el envío tradicional

                var token = $('input[name="__RequestVerificationToken"]').val(); // Obtener el token CSRF


                idDepartamentoSeleccionado = $("#offcanvasNombreDepartamento").val();
                if(formEditar.originalValues.nombre != idDepartamentoSeleccionado){//Si cambio el departamento seleccionado
                    idOrganigrama = $("#offcanvasIdOrganigramaDepartamento").val();
                    if (tieneHijosNodo(parseInt(idOrganigrama))) {
                        messageAlert("Error", "No puedes cambiar el departamento que tiene hijos asignados.", "error", "Aceptar")
                            .then(() => {
                                overlay.style.display = "none";
                                overlay.style.zIndex = "1060";
                            });
                        return false;
                    }
                }


                //Validaciones
                if (!$("#offcanvasNombreDepartamento").val() && !$("#offcanvasNombreDepartamentoNuevo").val()) {
                    messageAlert("Error", "Debes seleccionar un departamento o escribir un nuevo departamento.", "error", "Aceptar")
                        .then(() => {
                            overlay.style.display = "none";
                            overlay.style.zIndex = "1060";
                        }
                    );

                    return false;
                }

                var formData = new FormData();
                formData.append("Id", $("#offcanvasIdOrganigramaDepartamento").val());
                formData.append("IdDepartamento.Id", $("#offcanvasNombreDepartamento").val() || null);
                formData.append("IdDepartamento.Nombre", $("#offcanvasNombreDepartamentoNuevo").val() || "");

                // Obtener el archivo seleccionado
                var fileInput = $("#documentoFotoPerfilDepartamento")[0];  // Asumiendo que el id del input file es "inputFile"
                if (fileInput.files.length > 0) {
                    formData.append("IdDocumento.Archivo", fileInput.files[0]); // Nombre de la propiedad en el modelo
                }

                // Los demás datos
                formData.append("IdDocumento.Id", $("#documentoFotoPerfilDepartamento_Id").val());
                formData.append("IdDocumento.IdTipoDocumento", 36); // Suponiendo un valor para IdTipoDocumento

                //IMprimir valores del formData
                for (var pair of formData.entries()) {
                    console.log(pair[0] + ', ' + pair[1]);
                }


                $.ajax({
                    type: "POST",
                    url: `/Organigrama?handler=EditarNodoDepartamento`,  // Ajusta según tu Razor Page
                    data: formData,
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN", token); // Agregar token CSRF
                    },
                    processData: false, // No procesar los datos
                    contentType: false, // No establecer el contentType, porque FormData se encarga de eso
                    dataType: "json"
                }).done(function (data) {
                    if(data.success){
                        messageAlert("Guardado", "La información del departamento ha sido guardada.", "success", "Aceptar")
                            .then(() => {
                                overlay.style.display = "none";
                                overlay.style.zIndex = "1060";
                                peticionDatosOrganigrama();
                                $('#DepartamentoModal').offcanvas('hide');
                            });
                    } else {    
                        messageAlert("Error", "Hubo un problema al guardar la información del departamento.", "error", "Aceptar")
                            .then(() => {
                                overlay.style.display = "none";
                                overlay.style.zIndex = "1060";
                            }
                        );
                    }
                }).fail(function (xhr, status, error) {
                    messageAlert("Error", "Hubo un problema al guardar la información del departamento.", "error", "Aceptar")
                        .then(() => {
                            overlay.style.display = "none";
                            overlay.style.zIndex = "1060";
                        });
                    console.error("Error en la petición:", status, error);
                });

                return false;
            });
        });


        //Actualizar input de documentos si cambio.
        document.getElementById("documentoFotoPerfilDepartamento").addEventListener("change", (event) => {
            const input = event.target;
            const file = input.files[0];

            if (file) {
                const id = input.id; //El input poculto tinee el mismo id actual per agregado _Id
                const inputOculto = document.getElementById(`${id}_Id`); // Obtener el input oculto para cambiarlo a negativo si se edito
                if (inputOculto) {
                    inputOculto.value *= -1;
                }
            }
        });

        // Mostrar input si se elige "Nuevo Puesto"
        document.getElementById("offcanvasNombreDepartamento").addEventListener("change", function () {
            let nuevoUsuarioInput = document.getElementById("offcanvasNombreDepartamentoNuevo");
            if (this.value === "") {
                nuevoUsuarioInput.style.display = "inline-block";   
            } else {
                nuevoUsuarioInput.style.display = "none";
                nuevoUsuarioInput.value = "";
            }
        });



        /*--------------------------------------------------------------------------------------MOdal exportar*/

        const exampleModal = document.getElementById('exportarModal')
        if (exampleModal) {
          exampleModal.addEventListener('show.bs.modal', event => {
            // Button that triggered the modal
            const button = event.relatedTarget
            // Extract info from data-bs-* attributes
            const recipient = button.getAttribute('data-bs-contenedor')
            const elementosOcultar = button.getAttribute('data-bs-elementos-ocultar')

            const inputContenedorModal = exampleModal.querySelector('#contenedorExportar')
            const inputElementosOcultar = exampleModal.querySelector('#elementoOcultarAntesExportar')

            if(recipient != "tree"){
                //Quotar option de exportar excel
                document.getElementById("tipoExportar").querySelector("option[value='excel']").disabled = true;
                //Señleccionar otra opcion
                document.getElementById("tipoExportar").value = "png";

            } else {
                document.getElementById("tipoExportar").querySelector("option[value='excel']").disabled = false;
            }
        
            inputContenedorModal.value = recipient
            inputElementosOcultar.value = elementosOcultar
          })
        }



        function changeExportType(select) {
            
            const exportType = select;
            document.getElementById("contenedorHeaderFooter").style.display = "none";
            
            if(exportType === "pdfConHeaderFooter"){
                document.getElementById("contenedorHeaderFooter").style.display = "block";
            }
            
        }

        function exportar(){
            const tipoExportar = document.getElementById('tipoExportar').value;
            const headerText = document.getElementById('textHeader').value;
            const footerText = document.getElementById('textFooter').value;
            const contenedor = document.getElementById('contenedorExportar').value;
            const elementosOcultar = JSON.parse(document.getElementById('elementoOcultarAntesExportar').value); 

            if(tipoExportar == "pdf"){
                exportarContenedorPDF(contenedor, elementosOcultar);
            } else if(tipoExportar == "png"){
                exportarContenedorPNG(contenedor, elementosOcultar);
            } else if(tipoExportar == "pdfConHeaderFooter"){
                exportarContenedorPDFConHeaderFooter(contenedor, headerText, footerText, elementosOcultar);
            } else if(tipoExportar == "excel"){
                exportarDatosExcel();
            }
        }



        function exportarContenedorPDF(contenedorId, elementosOcultar) {
            const contenedor = document.getElementById(contenedorId);
        
            console.log("Iniciando exportación...");
            if (!contenedor) {
                console.error(`El contenedor #${contenedorId} no existe.`);
                return;
            }

            // Asegurar que todo el contenido es visible antes de capturarlo
            contenedor.style.overflow = "visible";
            
        
            setTimeout(() => {
                html2canvas(contenedor, {
                    ignoreElements: function(element) {
                        // Si el elemento está en la lista de elementos a ocultar, lo ignoramos
                        return elementosOcultar.some(selector => element.matches(selector));
                    },
                    scale: 6, // Mejora la calidad de la imagen
                    useCORS: true,
                    // Captura el contenido completo aunque esté fuera del viewport
                    windowHeight: contenedor.scrollHeight,
                    scrollX: 100,
                    scrollY: -window.scrollY + 100
                }).then(canvas => {
                    console.log("Canvas generado correctamente.");
        
                    const imgData = canvas.toDataURL('image/png');
        
                    // Convertir el tamaño del canvas de píxeles a mm (1 px ≈ 0.264583 mm)
                    const pxToMm = 0.264583 / 4; // Ajuste para que la imagen no sea gigante
                    const imgWidth = canvas.width * pxToMm;
                    const imgHeight = canvas.height * pxToMm;
        
                    const { jsPDF } = window.jspdf;
                    const pdf = new jsPDF({
                        orientation: imgWidth > imgHeight ? 'landscape' : 'portrait',
                        unit: 'mm',
                        format: [imgWidth, imgHeight] // Ajustar el tamaño del PDF al de la imagen
                    });
        

                    // Agregar la imagen del organigrama al PDF
                    pdf.addImage(imgData, 'PNG', 0, 0, imgWidth, imgHeight);

                    // Buscar el enlace en el contenedor
                    const enlace = contenedor.querySelector('a'); // Encuentra el primer enlace
                    if (enlace) {
                        // Obtener la URL del enlace
                        const url = enlace.href;

                        // Agregar el enlace al PDF, manteniendo la interactividad
                        // Ajustar el texto y las coordenadas
                        pdf.setFontSize(10);
                        pdf.text('', 7, 248); 

                        // Hacer que la URL sea clickeable en el PDF
                        pdf.link(7, 248, 80, 10, { url: url });
                    }

                    pdf.save('Organigrama.pdf');
                    console.log("PDF generado y descargado.");
                }).catch(error => {
                    console.error("Error al generar el canvas:", error);
                });
            }, 500);
        }





        function exportarContenedorPDFConHeaderFooter(contenedorId, headerText, footerText, elementosOcultar) {
            const contenedor = document.getElementById(contenedorId);
        
            console.log("Iniciando exportación...");
            if (!contenedor) {
                console.error(`El contenedor #${contenedorId} no existe.`);
                return;
            }

            // Asegurar que todo el contenido es visible antes de capturarlo
            contenedor.style.overflow = "visible";
        
            setTimeout(() => {
                html2canvas(contenedor, {
                    ignoreElements: function(element) {
                        // Si el elemento está en la lista de elementos a ocultar, lo ignoramos
                        return elementosOcultar.some(selector => element.matches(selector));
                    },
                    scale: 6, // Mejora la calidad de la imagen
                    useCORS: true,
                    // Captura el contenido completo aunque esté fuera del viewport
                    windowHeight: contenedor.scrollHeight,
                    scrollX: 100,
                    scrollY: -window.scrollY + 100
                }).then(canvas => {
                    console.log("Canvas generado correctamente.");
        
                    const imgData = canvas.toDataURL('image/png');
        
                    // Convertir el tamaño del canvas de píxeles a mm (1 px ≈ 0.264583 mm), luego dividiendolo par auqe no este inmeso y no pese mucho
                    const imgWidth = (canvas.width * 0.264583) / 3;
                    const imgHeight = (canvas.height * 0.264583) / 3;
                    const headerHeight = 15; 
                    const footerHeight = 15; 
        
                    const { jsPDF } = window.jspdf;
                    const pdf = new jsPDF({
                        orientation: imgWidth > imgHeight ? 'landscape' : 'portrait',
                        unit: 'mm',
                        format: [imgWidth, imgHeight + headerHeight + footerHeight] // Ajustar el tamaño del PDF al de la imagen
                    });
        
                    // Agregar el encabezado (Header)
                    pdf.setFontSize(10);
                    pdf.setTextColor(100);
                    pdf.text(headerText, 5, 10);

                    // Agregar la imagen del organigrama debajo del header
                    pdf.addImage(imgData, 'PNG', 0, headerHeight, imgWidth, imgHeight);

                    // Agregar el pie de página (Footer)
                    pdf.setFontSize(10);
                    pdf.setTextColor(100);
                    pdf.text(footerText, 5, imgHeight + headerHeight + 10);

                    // Buscar el enlace en el contenedor
                    const enlace = contenedor.querySelector('a'); // Encuentra el primer enlace
                    if (enlace) {
                        // Obtener la URL del enlace
                        const url = enlace.href;

                        // Agregar el enlace al PDF, manteniendo la interactividad
                        // Ajustar el texto y las coordenadas
                        pdf.setFontSize(10);
                        pdf.text('', 7, 240 + headerHeight);  // El texto del enlace, ajustado con un desplazamiento

                        // Hacer que la URL sea clickeable en el PDF
                        pdf.link(7, 240 + headerHeight, 80, 10, { url: url });
                    }


                    pdf.save('ArchivoConFooterHeader.pdf');
                    console.log("PDF generado y descargado.");
                }).catch(error => {
                    console.error("Error al generar el canvas:", error);
                });
            }, 500);
        }
        

        
        function exportarContenedorPNG(contenedorId, elementosOcultar) {
            const contenedor = document.getElementById(contenedorId);
        
            console.log("Iniciando exportación...");
            if (!contenedor) {
            console.error("El contenedor #tree no existe.");
            return;
            }

            // Asegurar que todo el contenido es visible antes de capturarlo
            contenedor.style.overflow = "visible";
        
            setTimeout(() => {
            html2canvas(contenedor, {
                ignoreElements: function(element) {
                    // Si el elemento está en la lista de elementos a ocultar, lo ignoramos
                    return elementosOcultar.some(selector => element.matches(selector));
                },
                scale: 5,
                useCORS: true,
                // Captura el contenido completo aunque esté fuera del viewport
                windowHeight: contenedor.scrollHeight,
                scrollX: 100,
                scrollY: -window.scrollY + 100
                }).then(canvas => {   console.log("Canvas generado correctamente.");
        
                const imgData = canvas.toDataURL('image/png');
        
                // Descargar PNG
                const link = document.createElement('a');
                link.href = imgData;
                link.download = 'Archivo.png';
                link.click();
                console.log("PNG generado y descargado.");
            }).catch(error => {
                console.error("Error al generar el canvas:", error);
            });
            }, 500);
        }
        


        function exportarDatosExcel() {
            fetch(`/Organigrama?handler=ExportarOrganigrama`, {
                method: "GET"
            })
            .then(response => {
                const contentType = response.headers.get("Content-Type");
                if (!response.ok || !contentType.includes("spreadsheet")) {
                    throw new Error("No se recibió un archivo válido.");
                }
                return response.blob();
            })
            .then(blob => {
                let link = document.createElement("a");
                link.href = URL.createObjectURL(blob);
                link.download = "Organigrama.xlsx";
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            })
            .catch(error => {
                console.error("Error al descargar el archivo:", error);
                alert("Hubo un problema al generar el archivo.");
            });
        }
        





/* --------------------------------------------------------------------------------------------FUncion extra*/


function messageAlert(title, text, icon, confirmButtonText) {
    overlay.style.display = "block";
    overlay.style.zIndex = "12000";

    return Swal.fire({
        title: title,
        text: text,
        icon: icon,
        confirmButtonText: confirmButtonText,
        didOpen: () => {
            const swalModal = document.querySelector(".swal2-container");
            const swalContainer = document.getElementById("swal-fixed-container");

            if (swalModal && swalContainer) {
                swalModal.classList.add("swal2-superior");
                swalContainer.appendChild(swalModal);
                swalModal.style.position = "absolute";
                swalModal.style.top = "50%";
                swalModal.style.left = "50%";
                swalModal.style.transform = "translate(-50%, -50%)";
                swalModal.style.width = "100%";
            }

            document.querySelector(".swal2-container").classList.add("swal2-backdrop-hide");
            document.querySelector(".swal2-container").classList.remove("swal2-backdrop-show");

            if (document.fullscreenElement) {
                document.querySelector(".swal2-container").classList.add("swal2-backdrop-show");
                document.querySelector(".swal2-container").classList.remove("swal2-backdrop-hide");
            }
        },
    });
}


