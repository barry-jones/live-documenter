using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.Issues
{
	public class Issue174
	{
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")]
        public virtual object InsertRecord(
                    ref global::System.Nullable<int> LessonId,
                    string Teacher,
                    global::System.Nullable<global::System.DateTime> DateEntered,
                    string QAType,
                    string ObservationType,
                    global::System.Nullable<int> Length,
                    string Observer,
                    global::System.Nullable<int> YearGroup,
                    string ClassCode,
                    global::System.Nullable<int> Boys,
                    global::System.Nullable<int> Girls,
                    string Subject,
                    global::System.Nullable<int> SetNo,
                    global::System.Nullable<int> SetOf,
                    string AssessmentRangeA,
                    string AssessmentRangeB,
                    global::System.Nullable<int> TimeOfDay,
                    string LessonPeriod,
                    string Support,
                    string FileName,
                    global::System.Nullable<int> BookNo,
                    global::System.Nullable<int> PageNo,
                    string FormName,
                    global::System.Nullable<int> FormNo,
                    string PenId,
                    global::System.Nullable<int> QASignature,
                    global::System.Nullable<int> StaffSignature,
                    global::System.Nullable<int> OverallJudgment,
                    string OJComments,
                    string DocumentName,
                    global::System.Nullable<bool> FollowUp,
                    global::System.Nullable<global::System.DateTime> DateForFollowUp,
                    string SummativeFutureDevelopment,
                    string SummativeDevelopmentStrategyText,
                    string SummativeStrategyBeReviewed,
                    string SummativeStrategyBeReviewedBy,
                    string TargetsFutureDevelopment,
                    string TargetsBeAchieved,
                    string TargetsBeReviewed) {
            global::System.Data.SqlClient.SqlCommand command = this.CommandCollection[2];
            if ((LessonId.HasValue == true)) {
                command.Parameters[1].Value = ((int)(LessonId.Value));
            }
            else {
                command.Parameters[1].Value = global::System.DBNull.Value;
            }
            if ((Teacher == null)) {
                command.Parameters[2].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[2].Value = ((string)(Teacher));
            }
            if ((DateEntered.HasValue == true)) {
                command.Parameters[3].Value = ((System.DateTime)(DateEntered.Value));
            }
            else {
                command.Parameters[3].Value = global::System.DBNull.Value;
            }
            if ((QAType == null)) {
                command.Parameters[4].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[4].Value = ((string)(QAType));
            }
            if ((ObservationType == null)) {
                command.Parameters[5].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[5].Value = ((string)(ObservationType));
            }
            if ((Length.HasValue == true)) {
                command.Parameters[6].Value = ((int)(Length.Value));
            }
            else {
                command.Parameters[6].Value = global::System.DBNull.Value;
            }
            if ((Observer == null)) {
                command.Parameters[7].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[7].Value = ((string)(Observer));
            }
            if ((YearGroup.HasValue == true)) {
                command.Parameters[8].Value = ((int)(YearGroup.Value));
            }
            else {
                command.Parameters[8].Value = global::System.DBNull.Value;
            }
            if ((ClassCode == null)) {
                command.Parameters[9].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[9].Value = ((string)(ClassCode));
            }
            if ((Boys.HasValue == true)) {
                command.Parameters[10].Value = ((int)(Boys.Value));
            }
            else {
                command.Parameters[10].Value = global::System.DBNull.Value;
            }
            if ((Girls.HasValue == true)) {
                command.Parameters[11].Value = ((int)(Girls.Value));
            }
            else {
                command.Parameters[11].Value = global::System.DBNull.Value;
            }
            if ((Subject == null)) {
                command.Parameters[12].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[12].Value = ((string)(Subject));
            }
            if ((SetNo.HasValue == true)) {
                command.Parameters[13].Value = ((int)(SetNo.Value));
            }
            else {
                command.Parameters[13].Value = global::System.DBNull.Value;
            }
            if ((SetOf.HasValue == true)) {
                command.Parameters[14].Value = ((int)(SetOf.Value));
            }
            else {
                command.Parameters[14].Value = global::System.DBNull.Value;
            }
            if ((AssessmentRangeA == null)) {
                command.Parameters[15].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[15].Value = ((string)(AssessmentRangeA));
            }
            if ((AssessmentRangeB == null)) {
                command.Parameters[16].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[16].Value = ((string)(AssessmentRangeB));
            }
            if ((TimeOfDay.HasValue == true)) {
                command.Parameters[17].Value = ((int)(TimeOfDay.Value));
            }
            else {
                command.Parameters[17].Value = global::System.DBNull.Value;
            }
            if ((LessonPeriod == null)) {
                command.Parameters[18].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[18].Value = ((string)(LessonPeriod));
            }
            if ((Support == null)) {
                command.Parameters[19].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[19].Value = ((string)(Support));
            }
            if ((FileName == null)) {
                command.Parameters[20].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[20].Value = ((string)(FileName));
            }
            if ((BookNo.HasValue == true)) {
                command.Parameters[21].Value = ((int)(BookNo.Value));
            }
            else {
                command.Parameters[21].Value = global::System.DBNull.Value;
            }
            if ((PageNo.HasValue == true)) {
                command.Parameters[22].Value = ((int)(PageNo.Value));
            }
            else {
                command.Parameters[22].Value = global::System.DBNull.Value;
            }
            if ((FormName == null)) {
                command.Parameters[23].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[23].Value = ((string)(FormName));
            }
            if ((FormNo.HasValue == true)) {
                command.Parameters[24].Value = ((int)(FormNo.Value));
            }
            else {
                command.Parameters[24].Value = global::System.DBNull.Value;
            }
            if ((PenId == null)) {
                command.Parameters[25].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[25].Value = ((string)(PenId));
            }
            if ((QASignature.HasValue == true)) {
                command.Parameters[26].Value = ((int)(QASignature.Value));
            }
            else {
                command.Parameters[26].Value = global::System.DBNull.Value;
            }
            if ((StaffSignature.HasValue == true)) {
                command.Parameters[27].Value = ((int)(StaffSignature.Value));
            }
            else {
                command.Parameters[27].Value = global::System.DBNull.Value;
            }
            if ((OverallJudgment.HasValue == true)) {
                command.Parameters[28].Value = ((int)(OverallJudgment.Value));
            }
            else {
                command.Parameters[28].Value = global::System.DBNull.Value;
            }
            if ((OJComments == null)) {
                command.Parameters[29].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[29].Value = ((string)(OJComments));
            }
            if ((DocumentName == null)) {
                command.Parameters[30].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[30].Value = ((string)(DocumentName));
            }
            if ((FollowUp.HasValue == true)) {
                command.Parameters[31].Value = ((bool)(FollowUp.Value));
            }
            else {
                command.Parameters[31].Value = global::System.DBNull.Value;
            }
            if ((DateForFollowUp.HasValue == true)) {
                command.Parameters[32].Value = ((System.DateTime)(DateForFollowUp.Value));
            }
            else {
                command.Parameters[32].Value = global::System.DBNull.Value;
            }
            if ((SummativeFutureDevelopment == null)) {
                command.Parameters[33].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[33].Value = ((string)(SummativeFutureDevelopment));
            }
            if ((SummativeDevelopmentStrategyText == null)) {
                command.Parameters[34].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[34].Value = ((string)(SummativeDevelopmentStrategyText));
            }
            if ((SummativeStrategyBeReviewed == null)) {
                command.Parameters[35].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[35].Value = ((string)(SummativeStrategyBeReviewed));
            }
            if ((SummativeStrategyBeReviewedBy == null)) {
                command.Parameters[36].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[36].Value = ((string)(SummativeStrategyBeReviewedBy));
            }
            if ((TargetsFutureDevelopment == null)) {
                command.Parameters[37].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[37].Value = ((string)(TargetsFutureDevelopment));
            }
            if ((TargetsBeAchieved == null)) {
                command.Parameters[38].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[38].Value = ((string)(TargetsBeAchieved));
            }
            if ((TargetsBeReviewed == null)) {
                command.Parameters[39].Value = global::System.DBNull.Value;
            }
            else {
                command.Parameters[39].Value = ((string)(TargetsBeReviewed));
            }
            global::System.Data.ConnectionState previousConnectionState = command.Connection.State;
            if (((command.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open)) {
                command.Connection.Open();
            }
            object returnValue;
            try {
                returnValue = command.ExecuteScalar();
            }
            finally {
                if ((previousConnectionState == global::System.Data.ConnectionState.Closed)) {
                    command.Connection.Close();
                }
            }
            if (((command.Parameters[1].Value == null)
                        || (command.Parameters[1].Value.GetType() == typeof(global::System.DBNull)))) {
                LessonId = new global::System.Nullable<int>();
            }
            else {
                LessonId = new global::System.Nullable<int>(((int)(command.Parameters[1].Value)));
            }
            if (((returnValue == null)
                        || (returnValue.GetType() == typeof(global::System.DBNull)))) {
                return null;
            }
            else {
                return ((object)(returnValue));
            }
        }

		public System.Data.SqlClient.SqlCommand[] CommandCollection { get; set; }
	}
}
