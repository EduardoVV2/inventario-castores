package com.grupocastores.inventario_castores.controller;

import java.util.Date;
import java.util.LinkedList;
import java.util.List;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

import com.grupocastores.inventario_castores.model.Producto;

import org.springframework.ui.Model;


@Controller
public class HomeController {

    @GetMapping("/productos")
    public String mostrarProductos(Model model) {
        List<Producto> productos = getProductos();
        model.addAttribute("productos", productos);
        return "productos"; // Retorna la vista de productos
    }

    @GetMapping("/")
    public String home(Model model) {
        model.addAttribute("title", "Inicio");
        model.addAttribute("fecha", new Date());
        return "home";
    }
    
    /**
     * Método que simula la obtención de una lista de productos.
     * @return Lista de productos.
     */
    public List<Producto> getProductos() {
        List<Producto> productos = new LinkedList<Producto>();
        Producto producto1 = new Producto();
        producto1.setIdProducto(1);
        producto1.setNombre("Producto A");
        producto1.setDescripcion("Descripción del Producto A");
        producto1.setCantidad(10);
        producto1.setActivo(true);
        productos.add(producto1);
        Producto producto2 = new Producto();
        producto2.setIdProducto(2);
        producto2.setNombre("Producto B");
        producto2.setDescripcion("Descripción del Producto B");
        producto2.setCantidad(20);
        producto2.setActivo(true);
        productos.add(producto2);
        Producto producto3 = new Producto();
        producto3.setIdProducto(3);
        producto3.setNombre("Producto C");
        producto3.setDescripcion("Descripción del Producto C");
        producto3.setCantidad(30);
        producto3.setActivo(false);
        productos.add(producto3);
        return productos;
    }

}
