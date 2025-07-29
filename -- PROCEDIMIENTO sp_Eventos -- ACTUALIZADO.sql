-- PROCEDIMIENTO sp_Eventos // ACTUALIZADO



ALTER PROCEDURE [dbo].[sp_Eventos]
(
    @Modo NVARCHAR(20),
    @EventoID INT = NULL,
    @MascotaID INT = NULL,
    @UsuarioID INT = NULL,
    @TipoEvento NVARCHAR(50) = NULL,
    @Titulo NVARCHAR(100) = NULL,
    @FechaHora DATETIME = NULL,
    @DoctorAsignado NVARCHAR(100) = NULL,
    @Descripcion NVARCHAR(MAX) = NULL,
    @EstadoID INT = NULL,
    @O_Numero INT = NULL OUTPUT,
    @O_Mensage VARCHAR(255) = NULL OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Modo = 'AGREGAR'
    BEGIN
        BEGIN TRY
            INSERT INTO tbl_Eventos (MascotaID, UsuarioID, TipoEvento, Titulo, FechaHora, DoctorAsignado, Descripcion, EstadoID)
            VALUES (@MascotaID, @UsuarioID, @TipoEvento, @Titulo, @FechaHora, @DoctorAsignado, @Descripcion, ISNULL(@EstadoID, 3));

            SET @O_Numero = 0;
            SET @O_Mensage = 'Evento agregado correctamente';
        END TRY
        BEGIN CATCH
            SET @O_Numero = ERROR_NUMBER();
            SET @O_Mensage = ERROR_MESSAGE();
        END CATCH
    END
    ELSE IF @Modo = 'ACTUALIZAR'
    BEGIN
        IF ISNULL(@EventoID, 0) = 0
        BEGIN
            SET @O_Numero = -1;
            SET @O_Mensage = 'El parametro EventoID no puede ser nulo';
        END
        ELSE
        BEGIN
            BEGIN TRAN UDP_EVENTO
            BEGIN TRY
                UPDATE tbl_Eventos
                SET MascotaID = COALESCE(@MascotaID, MascotaID),
                    UsuarioID = COALESCE(@UsuarioID, UsuarioID),
                    TipoEvento = COALESCE(@TipoEvento, TipoEvento),
                    Titulo = COALESCE(@Titulo, Titulo),
                    FechaHora = COALESCE(@FechaHora, FechaHora),
                    DoctorAsignado = COALESCE(@DoctorAsignado, DoctorAsignado),
                    Descripcion = COALESCE(@Descripcion, Descripcion),
                    EstadoID = COALESCE(@EstadoID, EstadoID)
                WHERE EventoID = @EventoID;

                COMMIT TRAN UDP_EVENTO;
                SET @O_Numero = 0;
                SET @O_Mensage = 'Evento actualizado correctamente';
            END TRY
            BEGIN CATCH
                ROLLBACK TRAN UDP_EVENTO;
                SET @O_Numero = ERROR_NUMBER();
                SET @O_Mensage = ERROR_MESSAGE();
            END CATCH
        END
    END
    ELSE IF @Modo = 'ELIMINAR'
    BEGIN
        DELETE FROM tbl_Eventos WHERE EventoID = @EventoID;
    END
    ELSE IF @Modo = 'LISTAR'
    BEGIN
        SELECT * FROM tbl_Eventos;
    END
    ELSE IF @Modo = 'BUSCAR'
    BEGIN
        SELECT * FROM tbl_Eventos WHERE EventoID = @EventoID;
    END
END