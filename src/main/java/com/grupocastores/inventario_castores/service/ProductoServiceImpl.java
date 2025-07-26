package com.grupocastores.inventario_castores.service;

import java.util.LinkedList;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.grupocastores.inventario_castores.model.Producto;
import com.grupocastores.inventario_castores.repository.ProductoRepository;

@Service
public class ProductoServiceImpl implements IProductoService{

    @Autowired
    private ProductoRepository productRepo;

    @Override
    public List<Producto> buscaProductos() {
        return productRepo.findAll();
    }

    @Override
    public Producto buscarPorId(Integer idProducto) {
        return productRepo.getById(idProducto);
    }

    @Override
    public Producto guardar(Producto producto) {
        return productRepo.save(producto);
    }

    @Override
    public void eliminarPorId(Integer id) {
        productRepo.deleteById(id);
    }

}
