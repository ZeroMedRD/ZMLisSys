UPDATE
    lisPatientLaboratoryMaster
SET
    lisPatientLaboratoryMaster.PTRowid = Table_B.id    
FROM
    lisPatientLaboratoryMaster AS Table_A
    INNER JOIN Patient AS Table_B
        ON Table_A.PLMPTIdno = Table_B.strIdno
--WHERE
--    Table_A.PTRowid = ''