package com.grupocastores.inventario_castores.controller;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;


@Controller
public class AdministradorController {
    
    @GetMapping("/inventarioAdmin")
    public String administrar() {
        return "productos";
    }
    

}
