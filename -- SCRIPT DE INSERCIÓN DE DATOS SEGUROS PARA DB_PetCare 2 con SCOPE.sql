



CREATE VIEW vw_HistorialCompletoMascotas AS
SELECT
    -- Datos del Dueño
    U.UsuarioID,
    U.NombreCompleto AS NombreDueño,
    U.Email,
    U.Telefono,
    U.FechaRegistro AS FechaRegistroDueño,
    EU.NombreEstado AS EstadoUsuario,
    
    -- Datos de la Mascota
    M.MascotaID,
    M.Nombre AS NombreMascota,
    M.Especie,
    M.Raza,
    M.FechaNacimiento,
    M.Edad,
    M.Sexo,
    M.Color,
    M.Peso,
    M.FotoURL AS FotoMascota,
    M.Notas AS NotasMascota,
    EM.NombreEstado AS EstadoMascota,
    
    -- Datos del Evento
    E.EventoID,
    E.TipoEvento,
    E.Titulo AS TituloEvento,
    E.FechaHora AS FechaHoraEvento,
    E.DoctorAsignado AS VeterinarioEvento,
    E.Descripcion AS DescripcionEvento,
    EE.NombreEstado AS EstadoEvento,
    
    -- Registro Médico
    RM.RegistroID,
    RM.TipoTratamiento AS TipoTratamientoPrincipal,
    RM.VeterinarioNombre AS VeterinarioRegistro,
    RM.Descripcion AS Diagnostico,
    ERM.NombreEstado AS EstadoRegistro,
    
    -- Tratamiento
    T.TratamientoID,
    T.TipoTratamiento AS TratamientoEspecifico,
    T.FechaAplicacion AS FechaTratamiento,
    T.ProximaDosis AS ProximaDosisTratamiento,
    T.Notas AS NotasTratamiento,
    ET.NombreEstado AS EstadoTratamiento,
    
    -- Medicamento
    MED.MedicamentoID,
    MED.Nombre AS NombreMedicamento,
    MED.Dosis,
    MED.Frecuencia,
    MED.FechaInicio AS InicioMedicamento,
    MED.FechaFin AS FinMedicamento,
    MED.DosisTomadas,
    MED.Notas AS NotasMedicamento,
    EMED.NombreEstado AS EstadoMedicamento,
    
    -- Vacuna
    V.VacunaID,
    V.Nombre AS NombreVacuna,
    V.FechaAplicacion AS FechaVacunacion,
    V.ProximaDosis AS ProximoRefuerzo,
    V.Notas AS NotasVacuna,
    EV.NombreEstado AS EstadoVacuna,
    
    -- Archivo Adjunto
    A.ArchivoID,
    A.NombreArchivo,
    A.TipoArchivo,
    A.FechaSubida,
    EA.NombreEstado AS EstadoArchivo

FROM tbl_Mascotas M
INNER JOIN tbl_Usuarios U ON M.UsuarioID = U.UsuarioID
INNER JOIN tbl_Estados EU ON U.EstadoID = EU.EstadoID
INNER JOIN tbl_Estados EM ON M.EstadoID = EM.EstadoID
LEFT JOIN tbl_Eventos E ON M.MascotaID = E.MascotaID
LEFT JOIN tbl_Estados EE ON E.EstadoID = EE.EstadoID
LEFT JOIN tbl_RegistrosMedicos RM ON E.EventoID = RM.EventoID
LEFT JOIN tbl_Estados ERM ON RM.EstadoID = ERM.EstadoID
LEFT JOIN tbl_Tratamientos T ON E.EventoID = T.EventoID AND M.MascotaID = T.MascotaID
LEFT JOIN tbl_Estados ET ON T.EstadoID = ET.EstadoID
LEFT JOIN tbl_Medicamentos MED ON RM.RegistroID = MED.RegistroID AND M.MascotaID = MED.MascotaID
LEFT JOIN tbl_Estados EMED ON MED.EstadoID = EMED.EstadoID
LEFT JOIN tbl_Vacunas V ON RM.RegistroID = V.RegistroID AND M.MascotaID = V.MascotaID
LEFT JOIN tbl_Estados EV ON V.EstadoID = EV.EstadoID
LEFT JOIN tbl_ArchivosAdjuntos A ON RM.RegistroID = A.RegistroID
LEFT JOIN tbl_Estados EA ON A.EstadoID = EA.EstadoID;