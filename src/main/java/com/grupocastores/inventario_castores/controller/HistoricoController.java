package com.grupocastores.inventario_castores.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

import com.grupocastores.inventario_castores.service.IMovimientoService;

import org.springframework.ui.Model;
import jakarta.servlet.http.HttpSession;

@Controller
public class HistoricoController {

    @Autowired
    private IMovimientoService serviceMovimiento;
    
    @GetMapping("/historico")
    public String historico(Model model, HttpSession session, RedirectAttributes redirectAttrs) {
        Integer rol = (Integer) session.getAttribute("rol");
        if (rol==null){
            redirectAttrs.addFlashAttribute("message", "No tienes los permisos para acceder a esta página.");
            redirectAttrs.addFlashAttribute("messageType", "error"); // error, info, success, etc.
            return "redirect:/";
        }
        else if (rol==1) {
            model.addAttribute("movimientos", serviceMovimiento.buscaMovimientos());
            return "historico";
        }

        else if (rol==2) {
            redirectAttrs.addFlashAttribute("message", "No tienes los permisos para acceder a esta página.");
            redirectAttrs.addFlashAttribute("messageType", "error"); // error, info, success, etc.
            return "redirect:/inventarioAlmacenista";
        }
        return "error";
    }

}
