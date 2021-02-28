insert into lisHospitalLaboratoryItem 
	select newid() as LHIRowid
		  ,'' as LLIRowid
		  ,lpd.PLDCode as HLICode
          ,lpd.PLDName as HLIName
		  ,'' as HLIDisplayRange
		  ,0 as HLIUpMale
		  ,0 as HLILoMale
		  ,0 as HLIUpFemale
		  ,0 as HLILoFemale
--		   ,lpd.PLDMemo 
	from lisPatientLaboratoryDetail lpd 
	group by 
	lpd.PLDCode
	,lpd.PLDName
	--,lpd.PLDMemo 
--order by lpd.PLDCode


select * from lisPatientLaboratoryDetail lpd 
join lisPatientLaboratoryMaster lpm on lpd.PLMRowid = lpd.PLMRowid 
where PLDCode='421'