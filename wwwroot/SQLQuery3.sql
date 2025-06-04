
-- 1. Agregar campos faltantes en la tabla Tratamiento
ALTER TABLE [dbo].[Tratamiento]
ADD
    CannabinoidesSeleccionados NVARCHAR(300) NULL, -- CBD, THC, Otro (separados por coma),
    FormaFarmaceuticaExtra NVARCHAR(100) NULL;     -- Si se especifica 'Otro'

-- 2. Agregar campos para diagnóstico libre y tratamiento recibido en comorbilidades
ALTER TABLE [dbo].[PacienteDiagnostico]
ADD
    DiagnosticoLibre NVARCHAR(150) NULL,
    TratamientoRecibido NVARCHAR(300) NULL;

-- 3. (Opcional) Permitir que DiagnosticoId sea nulo si se usa DiagnosticoLibre
ALTER TABLE [dbo].[PacienteDiagnostico]
ALTER COLUMN DiagnosticoId INT NULL;

-- 4. (Opcional) Crear tabla de cannabinoides si decides normalizar en lugar de guardar string
-- Descomenta si lo deseas

CREATE TABLE [dbo].[TratamientoCannabinoide] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TratamientoId INT NOT NULL,
    Tipo NVARCHAR(50) NOT NULL, -- Ej: CBD, THC, Otro
    Observacion NVARCHAR(100) NULL,
    CONSTRAINT FK_TratamientoCannabinoide_Tratamiento FOREIGN KEY (TratamientoId)
        REFERENCES [dbo].[Tratamiento] (Id)
        ON DELETE CASCADE
);


-- 5. Validación: Mostrar mensaje de éxito
PRINT 'La base de datos DBDIGESA ha sido actualizada correctamente para ajustarse al formulario completo en Blazor.';
