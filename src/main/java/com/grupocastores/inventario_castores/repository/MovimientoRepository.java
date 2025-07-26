package com.grupocastores.inventario_castores.repository;

import org.springframework.data.jpa.repository.JpaRepository;

import com.grupocastores.inventario_castores.model.Movimiento;

public interface MovimientoRepository extends JpaRepository<Movimiento, Integer> {
    
}
