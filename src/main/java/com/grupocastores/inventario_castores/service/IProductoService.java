package com.grupocastores.inventario_castores.service;

import java.util.List;

import com.grupocastores.inventario_castores.model.Producto;

public interface IProductoService {

    List<Producto> buscaProductos();

    Producto buscarPorId(Integer idProducto);
    
    Producto guardar(Producto producto);
    
    void eliminarPorId(Integer id);
}
