package com.grupocastores.inventario_castores.repository;

import org.springframework.data.jpa.repository.JpaRepository;

import com.grupocastores.inventario_castores.model.Producto;

public interface ProductoRepository extends JpaRepository<Producto, Integer> {
    
}
