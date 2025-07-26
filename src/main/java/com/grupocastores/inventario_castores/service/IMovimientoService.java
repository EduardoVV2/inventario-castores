package com.grupocastores.inventario_castores.service;

import java.util.List;

import com.grupocastores.inventario_castores.model.Movimiento;

public interface IMovimientoService {

    List<Movimiento> buscaMovimientos();

    Movimiento buscarPorId(Integer idMovimiento);
    
    Movimiento guardar(Movimiento movimiento);
    
    void eliminarPorId(Integer id);

}
