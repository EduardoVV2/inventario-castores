package com.grupocastores.inventario_castores.controller;

import java.time.LocalDateTime;

import org.hibernate.type.TrueFalseConverter;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

import com.grupocastores.inventario_castores.model.Movimiento;
import com.grupocastores.inventario_castores.model.Producto;
import com.grupocastores.inventario_castores.service.IMovimientoService;
import com.grupocastores.inventario_castores.service.IProductoService;
import com.grupocastores.inventario_castores.service.IUsuarioService;

import jakarta.servlet.http.HttpSession;

import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;



@Controller
public class AdministradorController {
    
    @Autowired
    private IProductoService serviceProducto;
    @Autowired
    private IMovimientoService serviceMovimiento;
    @Autowired
    private IUsuarioService serviceUsuario;
    
    @GetMapping("/inventarioAdmin")
    public String administrar(Model model, HttpSession session, RedirectAttributes redirectAttrs) {
        model.addAttribute("productos", serviceProducto.buscaProductos());
        Integer rol = (Integer) session.getAttribute("rol");
        if (rol==null){
            redirectAttrs.addFlashAttribute("message", "No tienes los permisos para acceder a esta página.");
            redirectAttrs.addFlashAttribute("messageType", "error"); // error, info, success, etc.
            return "redirect:/";
        }
        else if (rol==1) {
            return "administrador";
        }
        else if (rol==2) {
            redirectAttrs.addFlashAttribute("message", "No tienes los permisos para acceder a esta página.");
            redirectAttrs.addFlashAttribute("messageType", "error"); // error, info, success, etc.
            return "redirect:/inventarioAlmacenista";
        }
        return "error";
    }
    
    @PostMapping("/inventarioAdminGuardar")
    public String agregarProducto(RedirectAttributes redirectAttrs, @RequestParam("nombre") String nombre, @RequestParam("descripcion") String descripcion) {
        Producto nuevoProducto = new Producto();
        nuevoProducto.setNombre(nombre);
        nuevoProducto.setDescripcion(descripcion);
        nuevoProducto.setCantidad(0);
        nuevoProducto.setActivo(true);
        serviceProducto.guardar(nuevoProducto);
        redirectAttrs.addFlashAttribute("message", "Nuevo producto agregado correctamente.");
        redirectAttrs.addFlashAttribute("messageType", "success"); // error, info, success, etc.
        return "redirect:/inventarioAdmin";
    }
    
    @PostMapping("/inventarioAdminAgregar")
    public String inventarioAdminAgregar(HttpSession session, RedirectAttributes redirectAttrs, @RequestParam("id") Integer id, @RequestParam("cantidad") Integer cantidad) {
        Movimiento nuevoMovimiento = new Movimiento();
        Integer idUsuarioActual = (Integer) session.getAttribute("usuario");
        Producto editar = serviceProducto.buscarPorId(id);
        Integer cantidadFin = editar.getCantidad();
        if(cantidad<0){
            redirectAttrs.addFlashAttribute("message", "Inventario no se pudo agregar, numero invalido.");
            redirectAttrs.addFlashAttribute("messageType", "error"); // error, info, success, etc.
            return "redirect:/inventarioAdmin";
        }else{
            cantidadFin += cantidad;
        }
        editar.setCantidad(cantidadFin);
        serviceProducto.guardar(editar);
        nuevoMovimiento.setProducto(editar);
        nuevoMovimiento.setUsuario(serviceUsuario.buscarPorId(idUsuarioActual));
        nuevoMovimiento.setTipo("entrada");
        nuevoMovimiento.setCantidad(cantidad);
        nuevoMovimiento.setFechaHora(LocalDateTime.now());
        serviceMovimiento.guardar(nuevoMovimiento);
        redirectAttrs.addFlashAttribute("message", "Inventario agregado correctamente.");
        redirectAttrs.addFlashAttribute("messageType", "success"); // error, info, success, etc.
        return "redirect:/inventarioAdmin";
    }
    
    @PostMapping("/bajaProducto")
    public String bajaProducto(@RequestParam("idProduct") Integer id, RedirectAttributes redirectAttrs) {
        Producto editar = serviceProducto.buscarPorId(id);
        editar.setActivo(false);
        serviceProducto.guardar(editar);
        redirectAttrs.addFlashAttribute("message", "Producto dado de baja.");
        redirectAttrs.addFlashAttribute("messageType", "info"); // error, info, success, etc.
        return "redirect:/inventarioAdmin";
    }

    @PostMapping("/reactivarProducto")
    public String reactivarProducto(@RequestParam("idProduct") Integer id, RedirectAttributes redirectAttrs) {
        Producto editar = serviceProducto.buscarPorId(id);
        editar.setActivo(true);
        serviceProducto.guardar(editar);
        redirectAttrs.addFlashAttribute("message", "Producto restablecido.");
        redirectAttrs.addFlashAttribute("messageType", "info"); // error, info, success, etc.
        return "redirect:/inventarioAdmin";
    }
    

}
