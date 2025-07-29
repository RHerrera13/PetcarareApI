-- PROCEDIMIENTO sp_ArchivosAdjuntos -- ACTUALIZADO


ALTER PROCEDURE [dbo].[sp_ArchivosAdjuntos]
(
    @Modo NVARCHAR(20),
    @ArchivoID INT = NULL,
    @RegistroID INT = NULL,
    @NombreArchivo NVARCHAR(255) = NULL,
    @TipoArchivo NVARCHAR(50) = NULL,
    @FechaSubida DATETIME = NULL,
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
            INSERT INTO tbl_ArchivosAdjuntos (RegistroID, NombreArchivo, TipoArchivo, FechaSubida, EstadoID)
            VALUES (@RegistroID, @NombreArchivo, @TipoArchivo, ISNULL(@FechaSubida, GETDATE()), ISNULL(@EstadoID, 1));

            SET @O_Numero = 0;
            SET @O_Mensage = 'Archivo adjunto agregado correctamente';
        END TRY
        BEGIN CATCH
            SET @O_Numero = ERROR_NUMBER();
            SET @O_Mensage = ERROR_MESSAGE();
        END CATCH
    END
    ELSE IF @Modo = 'ACTUALIZAR'
    BEGIN
        IF ISNULL(@ArchivoID, 0) = 0
        BEGIN
            SET @O_Numero = -1;
            SET @O_Mensage = 'El parametro ArchivoID no puede ser nulo';
        END
        ELSE
        BEGIN
            BEGIN TRAN UDP_ARCHIVO
            BEGIN TRY
                UPDATE tbl_ArchivosAdjuntos
                SET RegistroID = COALESCE(@RegistroID, RegistroID),
                    NombreArchivo = COALESCE(@NombreArchivo, NombreArchivo),
                    TipoArchivo = COALESCE(@TipoArchivo, TipoArchivo),
                    FechaSubida = COALESCE(@FechaSubida, FechaSubida),
                    EstadoID = COALESCE(@EstadoID, EstadoID)
                WHERE ArchivoID = @ArchivoID;

                COMMIT TRAN UDP_ARCHIVO;
                SET @O_Numero = 0;
                SET @O_Mensage = 'Archivo adjunto actualizado correctamente';
            END TRY
            BEGIN CATCH
                ROLLBACK TRAN UDP_ARCHIVO;
                SET @O_Numero = ERROR_NUMBER();
                SET @O_Mensage = ERROR_MESSAGE();
            END CATCH
        END
    END
    ELSE IF @Modo = 'ELIMINAR'
    BEGIN
        DELETE FROM tbl_ArchivosAdjuntos WHERE ArchivoID = @ArchivoID;
    END
    ELSE IF @Modo = 'LISTAR'
    BEGIN
        SELECT * FROM tbl_ArchivosAdjuntos;
    END
    ELSE IF @Modo = 'BUSCAR'
    BEGIN
        SELECT * FROM tbl_ArchivosAdjuntos WHERE ArchivoID = @ArchivoID;
    END
END
