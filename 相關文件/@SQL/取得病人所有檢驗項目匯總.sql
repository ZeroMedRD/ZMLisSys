select lp.*,lhli.HLIName from 
(
	select lpld.PLDCode from 
	(
		select * from lisPatientLaboratoryMaster lplm where lplm.PTRowid = '8bb5060f-dcff-448f-8c59-8351802232a5'
	) as lplm join lisPatientLaboratoryDetail lpld on lplm.PLMRowid = lpld.PLMRowid
	group by lpld.PLDCode
) as lp
join lisHospitalLaboratoryItem lhli
on lp.PLDCode = lhli.HLICode
order by lp.PLDCode


-- LINQ 
var result = (from lplm in LisPatientLaboratoryMasters where lplm.PTRowid == "8bb5060f-dcff-448f-8c59-8351802232a5"
			 join lpld in LisPatientLaboratoryDetails on lplm.PLMRowid equals lpld.PLMRowid
			 select new { lpld.PLDCode }).Distinct().ToList();
			 
var r1 = from r in result join lhli in LisHospitalLaboratoryItems on r.PLDCode equals lhli.HLICode select new { r.PLDCode, lhli.HLIName };