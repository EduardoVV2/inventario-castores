package com.grupocastores.inventario_castores.controller;

import java.util.Date;
import java.util.LinkedList;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;

import com.grupocastores.inventario_castores.model.Producto;
import com.grupocastores.inventario_castores.model.Usuario;
import com.grupocastores.inventario_castores.service.IProductoService;
import com.grupocastores.inventario_castores.service.IUsuarioService;

import jakarta.servlet.http.HttpSession;

@Controller
public class HomeController {

    @Autowired
    private IProductoService serviceProducto;
    @Autowired
    private IUsuarioService serviceUsuario;

    @PostMapping("/login")
    public String login(@RequestParam("correo") String correo, @RequestParam("pswd") String pswd, HttpSession session) {
        List<Usuario> allUsuarios = serviceUsuario.buscaUsuarios();
        for (Usuario usr : allUsuarios) {
            if(usr.getCorreo().equals(correo)){
                if (usr.getContrasena().equals(pswd)) {
                    session.setAttribute("rol", usr.getJustIdRol());
                }
            }
        }
        Integer rol = (Integer) session.getAttribute("rol");
        if (rol==null){
            return "redirect:/";
        }
        if (rol==1) {
            return "redirect:/inventarioAdmin";
        }
        else if (rol==2) {
            return "redirect:/inventarioAlmacenista";
        }
        return "error";
    }
    

    @GetMapping("/productos")
    public String mostrarProductos(Model model) {
        List<Producto> productos = serviceProducto.buscaProductos();
        model.addAttribute("productos", productos);
        return "productos"; // Retorna la vista de productos
    }

    @GetMapping("/")
    public String home(Model model) {
        model.addAttribute("title", "Inicio");
        model.addAttribute("fecha", new Date());
        return "home";
    }

}
