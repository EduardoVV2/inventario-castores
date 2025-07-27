package com.grupocastores.inventario_castores.controller;

import java.util.List;
import java.time.LocalDateTime;
import java.util.ArrayList;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

import com.grupocastores.inventario_castores.model.Movimiento;
import com.grupocastores.inventario_castores.model.Producto;
import com.grupocastores.inventario_castores.model.Usuario;
import com.grupocastores.inventario_castores.service.IMovimientoService;
import com.grupocastores.inventario_castores.service.IProductoService;
import com.grupocastores.inventario_castores.service.IUsuarioService;

import jakarta.servlet.http.HttpSession;

import org.springframework.ui.Model;

@Controller
public class AlmacenistaController {

    @Autowired
    private IProductoService serviceProducto;
    @Autowired
    private IMovimientoService serviceMovimiento;
    @Autowired
    private IUsuarioService serviceUsuario;
    
    @GetMapping("/inventarioAlmacenista")
    public String inventarioGeneral(Model model, HttpSession session, RedirectAttributes redirectAttrs) {
        Integer rol = (Integer) session.getAttribute("rol");
        if (rol==null){
            redirectAttrs.addFlashAttribute("message", "No tienes los permisos para acceder a esta página.");
            redirectAttrs.addFlashAttribute("messageType", "error"); // error, info, success, etc.
            return "redirect:/";
        }
        else if (rol==1) {
            redirectAttrs.addFlashAttribute("message", "No tienes los permisos para acceder a esta página.");
            redirectAttrs.addFlashAttribute("messageType", "error"); // error, info, success, etc.
            return "redirect:/inventarioAdmin";
        }
        else if (rol==2) {
            List<Producto> soloActivos = new ArrayList<>();
            for (Producto producto : serviceProducto.buscaProductos()) {
                if (producto.getActivo()) {
                    soloActivos.add(producto);
                }
            };
            model.addAttribute("productos", soloActivos);
            return "almacenista";
        }
        return "error";
    }

    @PostMapping("/inventarioRestar")
    public String inventarioAdminAgregar(HttpSession session, @RequestParam("id") Integer id, @RequestParam("cantidad") Integer cantidad, RedirectAttributes redirectAttrs) {
        Movimiento nuevoMovimiento = new Movimiento();
        Integer idUsuarioActual = (Integer) session.getAttribute("usuario");
        Producto editar = serviceProducto.buscarPorId(id);
        Integer cantidadFin = editar.getCantidad();
        if( cantidad < 0 || cantidadFin < cantidad ){
            redirectAttrs.addFlashAttribute("message", "Inventario no restado, cantidad no valida.");
            redirectAttrs.addFlashAttribute("messageType", "error"); // error, info, success, etc.
            return "redirect:/inventarioAlmacenista";
        }else{
            cantidadFin -= cantidad;
        }
        editar.setCantidad(cantidadFin);
        serviceProducto.guardar(editar);
        nuevoMovimiento.setProducto(editar);
        nuevoMovimiento.setUsuario(serviceUsuario.buscarPorId(idUsuarioActual));
        nuevoMovimiento.setTipo("salida");
        nuevoMovimiento.setCantidad(cantidad);
        nuevoMovimiento.setFechaHora(LocalDateTime.now());
        serviceMovimiento.guardar(nuevoMovimiento);
        redirectAttrs.addFlashAttribute("message", "Inventario restado correctamente.");
        redirectAttrs.addFlashAttribute("messageType", "success"); // error, info, success, etc.
        return "redirect:/inventarioAlmacenista";
    }

}
