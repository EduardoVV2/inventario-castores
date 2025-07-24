package com.grupocastores.inventario_castores.controller;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

@Controller
public class AlmacenistaController {
    
    @GetMapping("/inventarioAlmacenista")
    public String inventarioGeneral() {
        return "productos";
    }

}
