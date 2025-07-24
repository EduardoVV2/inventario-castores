package com.grupocastores.inventario_castores.service;

import java.util.LinkedList;
import java.util.List;

import org.springframework.stereotype.Service;

import com.grupocastores.inventario_castores.model.Producto;

@Service
public class ProductoServiceImpl implements IProductoService{

    private List<Producto> productos = null;

    public ProductoServiceImpl(){

        productos = new LinkedList<Producto>();

        Producto producto1 = new Producto();
        producto1.setIdProducto(1);
        producto1.setNombre("Producto Chido 1");
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
    }

    public List<Producto> buscaProductos() {
        return productos;
    }
    
}
