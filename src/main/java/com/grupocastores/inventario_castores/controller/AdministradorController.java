package com.grupocastores.inventario_castores.controller;

import org.hibernate.type.TrueFalseConverter;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestParam;

import com.grupocastores.inventario_castores.model.Producto;
import com.grupocastores.inventario_castores.service.IProductoService;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;



@Controller
public class AdministradorController {
    
    @Autowired
    private IProductoService serviceProducto;
    
    @GetMapping("/inventarioAdmin")
    public String administrar(Model model) {
        model.addAttribute("productos", serviceProducto.buscaProductos());
        return "administrador";
    }
    
    @PostMapping("/inventarioAdminGuardar")
    public String agregarProducto(Model model, @RequestParam("nombre") String nombre, @RequestParam("descripcion") String descripcion) {
        Producto nuevoProducto = new Producto();
        nuevoProducto.setNombre(nombre);
        nuevoProducto.setDescripcion(descripcion);
        nuevoProducto.setCantidad(0);
        nuevoProducto.setActivo(true);
        serviceProducto.guardar(nuevoProducto);
        model.addAttribute("productos", serviceProducto.buscaProductos());
        return "redirect:/inventarioAdmin";
    }
    
    @PostMapping("/inventarioAdminAgregar")
    public String inventarioAdminAgregar(@RequestParam("id") Integer id, @RequestParam("cantidad") Integer cantidad) {
        Producto editar = serviceProducto.buscarPorId(id);
        Integer cantidadFin = editar.getCantidad();
        if(cantidad<0){
            return "error";
        }else{
            cantidadFin += cantidad;
        }
        editar.setCantidad(cantidadFin);
        serviceProducto.guardar(editar);
        return "redirect:/inventarioAdmin";
    }
    
    @PostMapping("/bajaProducto")
    public String bajaProducto(@RequestParam("idProduct") Integer id) {
        Producto editar = serviceProducto.buscarPorId(id);
        editar.setActivo(false);
        serviceProducto.guardar(editar);
        return "redirect:/inventarioAdmin";
    }

    @PostMapping("/reactivarProducto")
    public String reactivarProducto(@RequestParam("idProduct") Integer id) {
        Producto editar = serviceProducto.buscarPorId(id);
        editar.setActivo(true);
        serviceProducto.guardar(editar);
        return "redirect:/inventarioAdmin";
    }
    

}
