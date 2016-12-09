$(document).ready(function () {
  /**
  * Modificando Puntero Control Menu Desplegable
  */
  if ($("body").attr("id") !== "tab_index") {
    $("#lbl_mante").css("cursor", "pointer");
    $("#lbl_cons").css("cursor", "pointer");
    $("#lbl_repo").css("cursor", "pointer");
  }

  /*
  * Llamado Metodo Para Mostrar Carga De Pantalla
  */
  carg_pagi(5);

  /**
  * Metodo Para Asignar Evento Enter A Caja
  */
  $("#txt_password").keydown(function (event) {
    var Code = event.keyCode;
    if (Code == 13) {
      IniciarSesion();
    }
  });
});

/*Inicializando Variables Globales Para Contenido De Modal*/
var Footer = "";
var Contenido = "";

/**
* Metodo Para Mostrar Recorrido Y Carga Inicio Pagina
* @param CantInterval Cantidad A Incrementar En Cada Iteracion
*/
function carg_pagi(CantInterval) {
  CantInterval += 15;
  $("#info_index").css("display", "none");
  $("#progress_index").css("display", "block");
  var BarraProgreso = $("#progressbar_index");
  var CantPorc = BarraProgreso.attr("porc_barr");
  if (CantPorc < 100) {
    if (CantInterval < 100) {
      $(".progress-inic").html(CantInterval + "%");
      BarraProgreso.css("width", CantInterval + "%");
      BarraProgreso.attr("porc_barr", CantInterval);
      setTimeout(function () { carg_pagi(CantInterval) }, 200);
    }
    else {
      /*Muestra Mensaje En Sesiones De Usuario*/
      if ($("body").attr("id") !== "tab_index") {
        Contenido = "<div class='text-center'><h1 class='text-primary'>Bienvenido!</h1></div>";
        Footer = "<button type='button' class='btn btn-primary' data-dismiss='modal'>Continuar</button>";
        PintaModal(Contenido, Footer, false)
        setTimeout(function () { $("#Message").modal("hide"); }, 1000);
      }
      $("#progress_index").css("display", "none");
      $("#info_index").css("display", "block");
    }
  }
  else {
    return false;
  }
}

/**
* Metodo Para Realizar Validacion Login Usuario
*/
function IniciarSesion() {
  var usuario = $("#txt_usuario").val();
  var password = $("#txt_password").val();
  if (usuario.length > 0 && password.length > 0) {
    var url = "/Logeo/ValidaUsuario";
    var data = { "Usuario": usuario, "Contraseña": password };
    EnviarPeticion(url, data);
  }
  else {
    Contenido = "Es Necesario Digitar Los Campos Requeridos Por Favor Verifique..!";
    PintaModal(Contenido, "", true);
  }
}

/**
* Metodo Para Iniciar Turno Operario
*/
function IniciarTurno() {
  var caja = $("#comb_nume_caja").val();
  var monto = $("#txt_mont_caja").val();
  var empleado = $("#hide_codi_empl").val();
  var observacion = $("#txt_obse_turn").val();
  var usuario = $("#txt_usuario").val();
  var password = $("#txt_password").val();
  var Campos = {
    "Caja": caja,
    "Monto": monto,
    "Usuario": empleado
  };

  if (ValidaCampo(Campos)) {
    var url = "/Logeo/IniciarTurno";
    var data = {
      "codi_caja": caja,
      "mont_caja": monto,
      "codi_empl": empleado,
      "desc_turn": observacion,
      "codi_usua": usuario,
      "clav_usua": password
    };
    EnviarPeticion(url, data);
  }
}

/**
* Metodo Para Validar Diligenciamiento De Campos
* @param Campos Objeto Con Campos A Validar
*/
function ValidaCampo(Campos) {
  console.log("Campos");
  console.log(Campos);
  for (var item in Campos) {
    if (!Campos[item]) {
      Contenido = "Es Necesario Diligenciar El Campo " + item + " Por Favor Verifique..!!";
      PintaModal(Contenido, "", true)
      return false;
    }
  }
  return true;
}

/*
* Metodo Para Enviar Peticion AJAX
* @Param Destino Direccion Peticion
* @Param Data Parametros Metodo Controlador
*/
function EnviarPeticion(Destino, Data) {
  $.ajax({
    url : Destino,
    data : Data,
    datatype : "String",
    type: "POST",    
    Asinc : false,
    beforeSend : function () {
      $("#load_tran").css("display", "block");
      $(document.body).css("cursor", "wait");
    }
  }).done(function (data) {
    $("#load_tran").css("display", "none");
    $(document.body).css("cursor", "default");
    var arregl_resp = data.split(",");
    var cant_oper = arregl_resp.length;
    for (var i = 0; i < cant_oper; i++) {
      var item = arregl_resp[i].toUpperCase();
      var content = arregl_resp[i + 1];
      if (content) {
        switch (item) {
          case "ERROR":
            Footer = "";
            PintaModal(content, Footer, true);
            break;
          case "MENSAJE":
            PintaModal(content, "", false);
            break;
          case "CONFIRMAR":
            var conf_info = content.split(';');
            Contenido = conf_info[0];
            Action = conf_info[1];
            MensajeConfirmacion(Action);
            break;
          case "LOCATION":
            location = content;
            break;
          default:
            //PintaModal(data, Footer, true);
            break;
        }
      }
    }
  });
}

/*
* Metodo Para Mostrar Confirmacion
* @param Action Accion A Dirigirse
*/
function MensajeConfirmacion(Action) {
  Footer = "<button type='button' class='btn btn-danger' data-dismiss='modal'>Cancelar &nbsp;<span class = 'glyphicon glyphicon-ban-circle'></span></button>";
  Footer += "<button type='button' class='btn btn-primary' onclick=" + Action + ">Aceptar &nbsp; <span class = 'glyphicon glyphicon-ok'></span></button>";
  PintaModal(Contenido, Footer, false);
}

/*
* Metodo Para Mostrar Modal
* @Param Content Contenido Modal
* @Param Footer Botones Inferiores Modal
* @param Error Indicador Error Ventana
*/
function PintaModal(Content, Footer, Error) {
  /*Definiendo Estilo Modal*/
  if (Error) {
    $(".modal-header").addClass("alert alert-danger");
    $(".modal-content").addClass("alert alert-danger");
  }
  else {
    $(".modal-header").removeClass("alert alert-danger");
    $(".modal-content").removeClass("alert alert-danger");
  }

  /*Definiendo Boton Por Defecto*/
  if (Footer == "") {
    Footer = "<button type='button' class='btn btn-danger' data-dismiss='modal'>Cerrar &nbsp;<span class = 'glyphicon glyphicon-ban-circle'></span></button>";
  }

  $(".modal-body").html();
  $(".modal-footer").html();
  $(".modal-body").html(Content);
  $(".modal-footer").html(Footer);
  $("#Message").modal("show");
}

/**
* Metodo Para Manipulacion De Active 
* @param data Informacion Componente Que Dispara El Evento
*/
function acti_ctrl(data) {
  var cont_menu = $(".nav.navbar-nav").children();
  for (var item in cont_menu) {
    $("#" + cont_menu[item].id).removeClass("active");
  }
  var ctrl_actu = data.id;
  $("#" + ctrl_actu).addClass("active");
}

/*
* Metodo Para Realizar Confirmacion Eliminar Del Empleado
* @param codi_empl Numero Empleado
*/
function ConfEliminaEmpleado(codi_empl) {
  Contenido = "Esta Seguro De Eliminar El Codigo Numero " + codi_empl;
  MensajeConfirmacion("EliminaEmpleado('" + codi_empl + "')");
}

/*
* Metodo Para Realizar Eliminacion Del Empleado
* @param codi_empl Numero Empleado
*/
function EliminaEmpleado(codi_empl) {
  var url = "/Empleado/EliminarEmpleado";
  var data = {
    "codi_empl": codi_empl
  };
  EnviarPeticion(url, data);
}

/*
* Metodo Para Realizar Confirmacion Eliminar De Caja
* @param codi_caja Numero Caja
*/
function ConfEliminaCaja(codi_caja) {
  Contenido = "Esta Seguro De Eliminar La Caja Numero " + codi_caja;
  MensajeConfirmacion("EliminaCaja('" + codi_caja + "')");
}

/*
* Metodo Para Realizar Eliminacion De Caja
* @param codi_caja Numero Caja
*/
function EliminaCaja(codi_caja) {
  var url = "/Cajas/EliminarCaja";
  var data = {
    "codi_caja": codi_caja
  };
  EnviarPeticion(url, data);
}

/*
* Metodo Para Realizar Confirmacion Eliminar De Privilegio
* @param codi_priv Numero Privilegio
*/
function ConfEliminaPriv(codi_priv) {
  Contenido = "Esta Seguro De Eliminar El Privilegio De Codigo " + codi_priv;
  MensajeConfirmacion("EliminaPriv('" + codi_priv + "')");
}

/*
* Metodo Para Realizar Eliminacion De Privilegio
* @param codi_priv Numero Privilegio
*/
function EliminaPriv(codi_priv) {
  var url = "/Privilegio_cliente/EliminarPriv";
  var data = {
    "codi_priv": codi_priv
  };
  EnviarPeticion(url, data);
}

/*
* Metodo Para Realizar Confirmacion Eliminar De Tipo Vehiculo
* @param tipo_vehi Tipo Vehiculo
*/
function ConfEliminaTipoV(tipo_vehi) {
  Contenido = "Esta Seguro De Eliminar El Tipo De Codigo " + tipo_vehi;
  MensajeConfirmacion("EliminaTipoV('" + tipo_vehi + "')");
}

/*
* Metodo Para Realizar Eliminacion De Tipo Vehiculo
* @param tipo_vehi Tipo Vehiculo
*/
function EliminaTipoV(tipo_vehi) {
  var url = "/Tipo_vehiculo/EliminarTipoV";
  var data = {
    "tipo_vehi": tipo_vehi
  };
  EnviarPeticion(url, data);
}

/**
* Metodo Para Modal De Captura Contenido Y Enviar Email Soporte Contactenos
*/
function moda_conta() {
  Contenido = "";
  Contenido += "<div class='panel panel-info'>";
  Contenido += "  <div class='panel-heading'>";
  Contenido += "    Contenido Email";
  Contenido += "  </div>";
  Contenido += "  <div class='panel-body'>";
  Contenido += "    <textarea class='form-control' id='cont_corr'>";
  Contenido += "    </textarea>";
  Contenido += "  </div>";
  Contenido += "  <div class='panel-footer text-center'>";
  Contenido += "    <button class='btn btn-info' id='envi_corr'>";
  Contenido += "      Enviar";
  Contenido += "      <span class='glyphicon glyphicon-share-alt'></span>";
  Contenido += "    </button>";
  //Contenido += "    <button class='btn btn-info' id='file_corr' type='button' onclick=\"$('#file_diag').trigger('click');\">";
  //Contenido += "      Adjuntar";
  //Contenido += "      <span class='glyphicon glyphicon-paperclip'></span>";
  //Contenido += "    </button>";
  //Contenido += "    <input id='file_diag' class='file' data-show-preview='false' type='file' style='display:none'>";
  Contenido += "  </div>";
  Contenido += "</div>";
  PintaModal(Contenido, Footer, false);
  $("#envi_corr").click(function () {
    var cont_corr = $("#cont_corr").val();
    var url = "/Home/Email";
    var data = {
      "cont_emai": cont_corr      
    };
    console.log(data);
    EnviarPeticion(url, data);
  });
}

/**
* Metodo Para Confirmar Cambiar Estado Lista Negra
* @param plac_vehi Placa Vehiculo 
*/
function ConfEstadoLista(plac_vehi) {
  Contenido = "Esta Seguro Cambiar De Estado A La Placa " + plac_vehi;
  MensajeConfirmacion("EstadoLista('" + plac_vehi + "')");
}

/**
* Metodo Para Cambiar Estado Placa Vehiulo
* @param plac_vehi Placa Vehiculo 
*/
function EstadoLista(plac_vehi) {
  var url = "/Lista_negra/EstadoLista";
  var data = {
    "plac_vehi": plac_vehi
  };
  EnviarPeticion(url, data);
}

/**
* Metodo Para Confirmacion Eliminar Cliente
* @param codi_clie Codigo Cliente
*/
function ConfElimCliente(codi_clie) {
  Contenido = "Esta Seguro Eliminar El Cliente De Codigo " + codi_clie;
  MensajeConfirmacion("EliminarCliente('" + codi_clie + "')");
}

/**
* Metodo Para Eliminar Cliente
* @param codi_clie Codigo Cliente
*/
function EliminarCliente(codi_clie) {
  var url = "/Clientes/EliminarCliente";
  var data = {
    "codi_clie": codi_clie
  };
  EnviarPeticion(url, data);
}

/**
* Metodo Para Mostrar Modal De Seleccion De Vehiculo
*/
function AsignarVehiculo() {
  var url = "/Clientes/MostrarVehiculos";
  var data = { "iden_clie": 99 };
  EnviarPeticion(url, data);
}

/**
* Metodo Para Crear Objeto Datos Cliente
*/
function DatoCliente() {
  var iden_clie = $("#iden_clie").val();
  var nomb_clie = $("#nomb_clie").val();
  var apel_clie = $("#apel_clie").val();
  var codi_priv = $("#codi_priv").val();

  var Campos = {
    "Identificacion": iden_clie,
    "Nombre": nomb_clie,
    "Apellido": apel_clie,
    "Privilegio": codi_priv
  };

  return Campos;
}

/**
* Metodo Para Realizar Creacion De Cliente Y Asignacion Vehiculo
*/
function AsignaVehiculo(plac_vehi) {
  /*Evaluacion De Datos*/
  var Campos = DatoCliente();

  if (ValidaCampo(Campos)) {
    var url = "/Clientes/GuardarCliente";
    var data = {
      "proc": "1",
      "iden_clie": Campos.Identificacion,
      "nomb_clie": Campos.Nombre,
      "apel_clie": Campos.Apellido,
      "codi_priv": Campos.Privilegio,
      "plac_vehi": plac_vehi,
      "colo_vehi": "",
      "obse_vehi": "",
      "marc_vehi": "",
      "tipo_vehi": "0",
      "codi_clie": "0"
    };
    EnviarPeticion(url, data);
  }
}

/**
* Metodo Para Ingresar Datos Vehiculo Nuevo
*/
function IngresarVehiculo() {
  var url = "/Clientes/IngresarVehiculo";
  EnviarPeticion(url, {});
}

/**
* Metodo Para Insertar vehicuilo
*/
function InsertarVehiculo() {
  /*Evaluacion De Datos Cliente*/
  var Campos = DatoCliente();
  if (ValidaCampo(Campos)) {
    /*Evaluacion De Datos Vehiculo*/
    var plac_vehi = $("#plac_vehi").val();
    var colo_vehi = $("#colo_vehi").val();
    var marc_vehi = $("#marc_vehi").val();
    var tipo_vehi = $("#tipo_vehi").val();
    var obse_vehi = $("#obse_vehi").val();
    var CamposVehi = {
      "Placa": plac_vehi,
      "Color": colo_vehi,
      "Marca": marc_vehi,
      "Tipo": tipo_vehi,
      "Observacion": obse_vehi
    };

    if (ValidaCampo(CamposVehi)) {
      var url = "/Clientes/GuardarCliente";
      var data = {
        "proc": "2",
        "iden_clie": Campos.Identificacion,
        "nomb_clie": Campos.Nombre,
        "apel_clie": Campos.Apellido,
        "codi_priv": Campos.Privilegio,
        "plac_vehi": plac_vehi,
        "colo_vehi": colo_vehi,
        "obse_vehi": obse_vehi,
        "marc_vehi": marc_vehi,
        "tipo_vehi": tipo_vehi,
        "codi_clie": "0"
      };
      EnviarPeticion(url, data);
    }
  }
}

/**
* Metodo Para Mostrar Vehiculos Asignados
*/
function VehiculosAsignados() {
  var iden_clie = $("#iden_clie").val();
  var url = "/Clientes/MostrarVehiculos";
  var data = { "iden_clie": iden_clie };
  EnviarPeticion(url, data);
}

/**
* Metodo Para Lanzar Confirmacion Eliminar Ubicacion
* @param codi_ubic Codigo Ubicacion
*/
function ConfEliminaUbic(codi_ubic)
{
  Contenido = "Esta Seguro Eliminar La Ubicacion De Codigo " + codi_ubic;
  MensajeConfirmacion("EliminarUbicacion('" + codi_ubic + "')");
}

/**
* Metodo Para Eliminar Ubicacion
* @param codi_ubic Codigo Ubicacion
*/
function EliminarUbicacion(codi_ubic) {
  var url = "/Ubicacion/EliminarUbicacion";
  var data = {
    "codi_ubic": codi_ubic
  };
  EnviarPeticion(url, data);
}