USE [his3532042222]
GO

/****** Object:  Table [dbo].[lisHospitalLaboratoryItem]    Script Date: 2021/1/18 下午 02:17:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[lisHospitalLaboratoryItem](
	[HLIRowid] [nvarchar](128) NOT NULL,
	[LLIRowid] [nvarchar](128) NOT NULL,
	[HLICode] [nvarchar](20) NULL,
	[HLIDisplayRange] [nvarchar](100) NULL,
	[HLIUpMale] [float] NULL,
	[HLILoMale] [float] NULL,
	[HLIUpFemale] [float] NULL,
	[HLILoFemale] [float] NULL,
 CONSTRAINT [PK_HospitalLaboratoryItem] PRIMARY KEY CLUSTERED 
(
	[HLIRowid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[lisPatientLaboratoryDetail](
	[PLDRowid] [nvarchar](128) NOT NULL,
	[PLMRowid] [nvarchar](128) NOT NULL,
	[HLIRowid] [nvarchar](128) NOT NULL,
	[PLDCode] [nvarchar](30) NOT NULL,
	[PLDName] [nvarchar](255) NULL,
	[PLDValue] [float] NULL,
	[PLDStrValue] [nvarchar](50) NULL,
	[PLDType] [nvarchar](50) NULL,
	[PLDUnit] [nvarchar](20) NULL,
	[PLDMemo] [nvarchar](255) NULL,
	[PLDSeqno] [int] NOT NULL,
	[PLDInsertDate] [datetime] NULL,
 CONSTRAINT [PK_PatientLaboratoryDetail] PRIMARY KEY CLUSTERED 
(
	[PLDRowid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[lisPatientLaboratoryMaster](
	[PLMRowid] [nvarchar](128) NOT NULL,
	[PTRowid] [nvarchar](128) NULL,
	[PLMPTIdno] [nvarchar](10) NULL,
	[PLMPTName] [nvarchar](20) NULL,
	[PLMPTBirthday] [nvarchar](10) NULL,
	[PLMPTGender] [nvarchar](10) NULL,
	[PLMPTCode] [nvarchar](20) NULL,
	[PLMClinicDate] [datetime] NULL,
	[PLMApplyDate] [datetime] NULL,
	[PLMApplyTime] [nvarchar](8) NULL,
	[PLMInspDate] [datetime] NULL,
	[PLMInspTime] [nvarchar](8) NULL,
	[PLMReportDate] [datetime] NULL,
	[PLMReportTime] [nvarchar](8) NULL,
	[PLMSNo] [nvarchar](50) NULL,
	[PLMReqno] [nvarchar](20) NULL,
	[PLMInsertDate] [datetime] NULL,
 CONSTRAINT [PK_PatientLaboratoryMaster] PRIMARY KEY CLUSTERED 
(
	[PLMRowid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

