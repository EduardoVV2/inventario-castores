package com.grupocastores.inventario_castores.controller;

import java.util.List;
import java.util.ArrayList;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;

import com.grupocastores.inventario_castores.model.Producto;
import com.grupocastores.inventario_castores.service.IProductoService;

import org.springframework.ui.Model;

@Controller
public class AlmacenistaController {

    @Autowired
    private IProductoService serviceProducto;
    
    @GetMapping("/inventarioAlmacenista")
    public String inventarioGeneral(Model model) {
        List<Producto> soloActivos = new ArrayList<>();
        for (Producto producto : serviceProducto.buscaProductos()) {
            if (producto.getActivo()) {
                soloActivos.add(producto);
            }
        };
        model.addAttribute("productos", soloActivos);
        return "almacenista";
    }

    @PostMapping("/inventarioRestar")
    public String inventarioAdminAgregar(@RequestParam("id") Integer id, @RequestParam("cantidad") Integer cantidad) {
        Producto editar = serviceProducto.buscarPorId(id);
        Integer cantidadFin = editar.getCantidad();
        if( cantidad < 0 || cantidadFin < cantidad ){
            return "error";
        }else{
            cantidadFin -= cantidad;
        }
        editar.setCantidad(cantidadFin);
        serviceProducto.guardar(editar);
        return "redirect:/inventarioAlmacenista";
    }

}
