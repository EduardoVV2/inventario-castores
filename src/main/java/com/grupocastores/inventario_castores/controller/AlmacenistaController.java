package com.grupocastores.inventario_castores.controller;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

import com.grupocastores.inventario_castores.model.Producto;
import com.grupocastores.inventario_castores.service.IProductoService;

import org.springframework.ui.Model;

@Controller
public class AlmacenistaController {

    @Autowired
    private IProductoService serviceProducto;
    
    @GetMapping("/inventarioAlmacenista")
    public String inventarioGeneral(Model model) {
        // List<Producto> soloActivos = new List<Producto>();
        // for (Producto producto : serviceProducto.buscaProductos()) {
        //     if (producto.getActivo()) {
        //         soloActivos.add(producto);
        //     }
        // };
        // model.addAttribute("productos", soloActivos);
        return "almacenista";
    }

}
