package com.grupocastores.inventario_castores.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;

import com.grupocastores.inventario_castores.service.IProductoService;


@Controller
public class AdministradorController {
    
    @Autowired
    private IProductoService serviceProducto;
    
    @GetMapping("/inventarioAdmin")
    public String administrar(Model model) {
        model.addAttribute("productos", serviceProducto.buscaProductos());
        return "productos";
    }
    

}
