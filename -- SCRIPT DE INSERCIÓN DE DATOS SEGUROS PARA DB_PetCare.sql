
-- SCRIPT DE INSERCIÓN DE DATOS SEGUROS PARA DB_PetCare

USE DB_PetCare;
GO

BEGIN TRANSACTION;

BEGIN TRY
    -- 1. Insertar Usuario
    INSERT INTO tbl_Usuarios (NombreCompleto, Email, Telefono, Contrasena, FechaRegistro, EstadoID)
    VALUES ('Carlos Mendoza', 'carlos@example.com', '8888-8888', CONVERT(VARBINARY(256), 'encryptedpass123'), GETDATE(), 1);

    DECLARE @UsuarioID INT = SCOPE_IDENTITY();

    -- 2. Insertar Mascota
    INSERT INTO tbl_Mascotas (UsuarioID, Nombre, Especie, Raza, FechaNacimiento, Sexo, Color, Peso, FotoURL, Notas, EstadoID)
    VALUES (@UsuarioID, 'Luna', 'Felino', 'Siames', '2022-02-10', 'Hembra', 'Gris', 4.2, 'C:\\Fotos\\Luna.jpg', 'Muy activa', 1);

    DECLARE @MascotaID INT = SCOPE_IDENTITY();

    -- 3. Insertar Evento
    INSERT INTO tbl_Eventos (MascotaID, UsuarioID, TipoEvento, Titulo, FechaHora, DoctorAsignado, Descripcion, EstadoID)
    VALUES (@MascotaID, @UsuarioID, 'Consulta', 'Revisión general', GETDATE(), 'Dra. Sofía Martínez', 'Sin anomalías detectadas', 1);

    DECLARE @EventoID INT = SCOPE_IDENTITY();

    -- 4. Insertar Registro Médico
    INSERT INTO tbl_RegistrosMedicos (EventoID, TipoTratamiento, VeterinarioNombre, Descripcion, EstadoID)
    VALUES (@EventoID, 'Chequeo General', 'Dra. Sofía Martínez', 'Chequeo físico sin hallazgos.', 1);

    DECLARE @RegistroID INT = SCOPE_IDENTITY();

    -- 5. Insertar Tratamiento
    INSERT INTO tbl_Tratamientos (MascotaID, EventoID, TipoTratamiento, FechaAplicacion, ProximaDosis, Notas, EstadoID)
    VALUES (@MascotaID, @EventoID, 'Desparasitación', '2025-06-23', '2025-09-01', 'Seguimiento en 3 meses', 1);

    -- 6. Insertar Medicamento
    INSERT INTO tbl_Medicamentos (MascotaID, RegistroID, Nombre, Dosis, Frecuencia, FechaInicio, FechaFin, DosisTomadas, EstadoID, Notas)
    VALUES (@MascotaID, @RegistroID, 'Albendazol', '200mg', '1 vez al día', '2025-06-23', '2025-06-30', 1, 1, 'Administrado sin efectos adversos');

    -- 7. Insertar Vacuna
    INSERT INTO tbl_Vacunas (MascotaID, RegistroID, Nombre, FechaAplicacion, ProximaDosis, Notas, EstadoID)
    VALUES (@MascotaID, @RegistroID, 'Triple Felina', '2025-06-23', '2026-06-01', 'Refuerzo anual requerido', 1);

    -- 8. Insertar Archivo Adjunto
    INSERT INTO tbl_ArchivosAdjuntos (RegistroID, NombreArchivo, TipoArchivo, FechaSubida, EstadoID)
    VALUES (@RegistroID, 'ChequeoGeneral_Luna.pdf', 'PDF', GETDATE(), 1);

    COMMIT;
    PRINT ' Todo insertado correctamente.';
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT ' Error: ' + ERROR_MESSAGE();
END CATCH;
