package com.grupocastores.inventario_castores.service;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.grupocastores.inventario_castores.model.Movimiento;
import com.grupocastores.inventario_castores.repository.MovimientoRepository;

@Service
public class MovimientoServiceImpl implements IMovimientoService{

    @Autowired
    private MovimientoRepository movimientoRepo;

    @Override
    public List<Movimiento> buscaMovimientos() {
        return movimientoRepo.findAll();
    }

    @Override
    public Movimiento buscarPorId(Integer idMovimiento) {
        return movimientoRepo.getById(idMovimiento);
    }

    @Override
    public Movimiento guardar(Movimiento movimiento) {
        return movimientoRepo.save(movimiento);
    }

    @Override
    public void eliminarPorId(Integer id) {
        movimientoRepo.deleteById(id);
    }
    
}
