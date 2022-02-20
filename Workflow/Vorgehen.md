# Procedure - Vorgehen

 1. create enumeration for the statuses - Enumeration für die Status anlegen
 2. create enumeration for the operations - Enumeration für die Operationen anlegen
 3. create domain class and implement IWorkflowParticipant - Domänenklasse anlegen und IWorkflowParticipant implementieren
 4. create workflow class for domain class - Workflow-Klasse für Domänenklasse anlegen
    - Implement abstract method GetOperations - Abstrakte Methode GetOperations implementieren
    - Implement abstract method GetNextState - Abstrakte Methode GetNextState implementieren