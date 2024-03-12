using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.Healthcare.PatientHealthcare.MedicalRecords
{
    public class MedicalRecordRepository
    {
        public const string MedicalRecordsFilePath = "..\\..\\..\\Healthcare\\PatientHealthcare\\MedicalRecords\\medical_records.csv";

        public  List<MedicalRecord> MedicalRecords = new();
        public  Serializer<MedicalRecord> MedicalRecordSerializer = new();

        public MedicalRecordRepository()
        {
            MedicalRecords = MedicalRecordSerializer.fromCSV(MedicalRecordsFilePath);
        }

        public  void SaveRepository()
        {
            MedicalRecordSerializer.toCSV(MedicalRecordsFilePath, MedicalRecords);
        }
        public  MedicalRecord? GetMedicalRecord(string patientUsername)
        {
            return MedicalRecords.FirstOrDefault(record => record.PatientUsername == patientUsername);
        }

        public  void EditMedicalRecord(string oldUsername, string newUsername, double height, double weight,
            DateOnly dateOfBirth, List<string> diseaseHistory, List<string>? allergies)
        {
            MedicalRecord? medicalRecord = GetMedicalRecord(oldUsername);

            medicalRecord!.PatientUsername = newUsername;
            medicalRecord.Height = height;
            medicalRecord.Weight = weight;
            medicalRecord.DateOfBirth = dateOfBirth;
            medicalRecord.DiseaseHistory = diseaseHistory;
            medicalRecord.Allergies = allergies?.ToList() ?? new List<string>();
            SaveRepository();
        }

        public  void EditMedicalRecord(MedicalRecord medicalRecord)
        {
            MedicalRecord? newMedicalRecord = GetMedicalRecord(medicalRecord.PatientUsername);

            newMedicalRecord.Height = medicalRecord.Height;
            newMedicalRecord.Weight = medicalRecord.Weight;
            newMedicalRecord.DateOfBirth = medicalRecord.DateOfBirth;
            newMedicalRecord.DiseaseHistory = medicalRecord.DiseaseHistory;

            SaveRepository();
        }

        public  void DeleteMedicalRecord(string patientsUsername)
        {
            MedicalRecord medicalRecord = GetMedicalRecord(patientsUsername)!;
            MedicalRecords.Remove(medicalRecord);
            SaveRepository();
        }

    }
}
