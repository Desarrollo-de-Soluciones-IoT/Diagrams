using System;
using Structurizr;
using Structurizr.Api;

namespace c4_model_design
{
    class Program
    {
        static void Main(string[] args)
        {
            Banking();
        }

        static void Banking()
        {
            const long workspaceId = 69652;
            const string apiKey = "67b26738-98b9-481d-ae7c-f6953341b8aa";
            const string apiSecret = "846efc0e-6b5f-4b0a-80df-9ed6983a9f8b";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("DocSeeker", "IOT");
            ViewSet viewSet = workspace.Views;
            Model model = workspace.Model;

            // 1. Diagrama de Contexto
            SoftwareSystem DocSeeker = model.AddSoftwareSystem("DocSeeker Application", "Aplicación que ofrece cubrir las necesidades básicas de atención médica a domicilio.");
            SoftwareSystem Payment = model.AddSoftwareSystem("Payment System", "Sistema de pago vía tarjeta de crédito/débito para recibir o transferir pagos.");
            SoftwareSystem Google = model.AddSoftwareSystem("Email System", "Mensajería electrónica mediante SMTP.");

            Person user = model.AddPerson("Paciente", "Persona que necesiten atención médico.");
            Person professional = model.AddPerson("Profesional de Salud", "Persona profesional de salud.");
            Person soporte = model.AddPerson("Soporte técnico", "Persona que Diagnóstico y solución de problemas técnicos en el BC.");

            user.Uses(DocSeeker, "Ingresa para buscar servicios de atención médica y comenzar a usar la aplicación web y mobile");
            professional.Uses(DocSeeker, "Ingresa para registrarse y usar la aplicación web y mobile");
            soporte.Uses(DocSeeker, "Ingresa para el monitoreo y mantenimiento del software y hardware utilizados en el BC");
            DocSeeker.Uses(Google, "Se ingresa para enviar emails de la aplicación web y mobile");
            DocSeeker.Uses(Payment, "Se integra para realizar pagos dentro de la aplicación web y mobile");

            SystemContextView contextView = viewSet.CreateSystemContextView(DocSeeker, "Contexto", "Context Diagram for DocSeeker");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // Tags
            user.AddTags("User");
            professional.AddTags("Professional");
            soporte.AddTags("Soport");
            DocSeeker.AddTags("MonitoringStation");
            Payment.AddTags("GoogleMaps");
            Google.AddTags("MonitoringStationSystem");

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("User") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("Professional") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("Soport") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("MonitoringStation") { Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("GoogleMaps") { Background = "#90714c", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("MonitoringStationSystem") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });

            // 2. Diagrama de Contenedores
            Container mobileApplication = DocSeeker.AddContainer("Mobile App", "Permite realizar las operaciones de los usuarios.", "Kotlin");
            Container webApplication = DocSeeker.AddContainer("Web App", "Permite a los usuarios interactuar con la UI y sus funcionalidades.", "Angular");
            Container landingPage = DocSeeker.AddContainer("Landing Page", "Permite a los usuarios de la aplicación interactuar con el landing, además de servir como una herramienta de marketing.", "HTML");
            Container apiRest = DocSeeker.AddContainer("API Gateway", "API Gateway", "SprintBoot port 8080");
            //////
            //Container PaymentContext = DocSeeker.AddContainer("Payment Bounded Context", "Bounded Context donde se realiza el método de pago.", "SprintBoot");
            Container MedicalAppointment = DocSeeker.AddContainer("Medical Appointment Bounded Context", "Bounded Context donde se enfoca en las citas solicitadas.", "SprintBoot");
            Container SecurityContext = DocSeeker.AddContainer("Security Bounded Context", "Bounded Context donde se manejan los servicios de verificación de cuentas de usuarios.", "SprintBoot");
            Container AuthenticationContext = DocSeeker.AddContainer("Authentication Bounded Context", "Bounded Context donde se manejan los servicios relacionados a la autenticación.", "SprintBoot");
            Container MessageContext = DocSeeker.AddContainer("Message Bounded Context", "Bounded Context donde se utilizará para gesitionar el sistema de mensajeria.", "SprintBoot");
            Container NotificationContext = DocSeeker.AddContainer("Temperature Notification Bounded Context", "Bounded Context donde se utilizará para medir la temperatura.", "SprintBoot");
            Container ProfileContext = DocSeeker.AddContainer("Profile Bounded Context", "Bounded Context donse se enfocará el apartado de perfiles.", "SprintBoot");
            Container Broker = DocSeeker.AddContainer("Broker", "Message Broker", "RabbitMQ");

            Container database = DocSeeker.AddContainer("Database", "", "MySQL");
            //relaciones
            //coloca hasta 3 argumentos
            user.Uses(mobileApplication, "Consult");
            user.Uses(webApplication, "Consult");
            user.Uses(landingPage, "Consult");
            professional.Uses(mobileApplication, "Consult");
            professional.Uses(webApplication, "Consult");
            professional.Uses(landingPage, "Consult");
            soporte.Uses(mobileApplication, "Consult");
            soporte.Uses(webApplication, "Consult");
            soporte.Uses(landingPage, "Consult");


            mobileApplication.Uses(apiRest, "API Request", "JSON/HTTPS");
            webApplication.Uses(apiRest, "API Request", "JSON/HTTPS");

            //apiRest.Uses(PaymentContext, "", "");
            apiRest.Uses(MedicalAppointment, "", "");
            apiRest.Uses(AuthenticationContext, "", "");
            apiRest.Uses(SecurityContext, "", "");
            apiRest.Uses(MessageContext, "", "");
            apiRest.Uses(NotificationContext, "", "");
            apiRest.Uses(ProfileContext, "", "");

            //PaymentContext.Uses(database, "", "JDBC");
            MedicalAppointment.Uses(Broker, "", "JDBC");
            SecurityContext.Uses(Broker, "", "JDB");
            MessageContext.Uses(Broker, "", "JDBC");
            NotificationContext.Uses(Broker, "", "JDBC");
            AuthenticationContext.Uses(Broker, "", "JDBC");
            ProfileContext.Uses(Broker, "", "JDBC");
            Broker.Uses(database, "", "JDBC");

            AuthenticationContext.Uses(Payment, "API Request", "JSON/HTTPS");
            SecurityContext.Uses(Google, "API Request", "JSON/HTTPS");

            // Tags
            mobileApplication.AddTags("MobileApp");
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiRest.AddTags("APIRest");
            database.AddTags("Database");
            MedicalAppointment.AddTags("MedicalAppointment");
            SecurityContext.AddTags("SecurityContext");
            AuthenticationContext.AddTags("AuthenticationContext");
            MessageContext.AddTags("MessageContext");
            NotificationContext.AddTags("NotificationContext");
            ProfileContext.AddTags("ProfileContext");
            Broker.AddTags("Broker");

            styles.Add(new ElementStyle("MobileApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("APIRest") { Shape = Shape.RoundedBox, Background = "#0000ff", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("Database") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("FlightMedicalAppointment") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ReservationContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ServiceContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MedicalAppointment") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("SecurityContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AuthenticationContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MessageContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ProfileContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });

            styles.Add(new ElementStyle("NotificationContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("Broker") { Shape = Shape.Cylinder, Background = "#9CFA61", Icon = "" });

            styles.Add(new ElementStyle("CardContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ChatContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });

            ContainerView containerView = viewSet.CreateContainerView(DocSeeker, "Contenedor", "Container Digram for DocSeeker");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();

            // 3. Diagrama de Componentes
            Component AuthenticationController = MedicalAppointment.AddComponent("Location Controller", "Permite usar los servicios de ubicación de los servicios de agencias.", ".NET Controller");
            Component PlanApplicationService = MedicalAppointment.AddComponent("Location Service", "Permite usar el servicio de ubicación de la APi", ".NET Controller");
            Component PlanRepository = MedicalAppointment.AddComponent("Location Repository", "Permite la comunicación entre el servicio de ubicación y la base de datos.", ".NET Controller");
            landingPage.Uses(AuthenticationController, "", "JSON/HTTPS");

            AuthenticationController.Uses(PlanApplicationService, "Administra");

            PlanRepository.Uses(database, "Lee y escribe datos", "JDBC");
            PlanApplicationService.Uses(PlanRepository, "Se apoya de");

            // Tags
            AuthenticationController.AddTags("CardController");
            PlanApplicationService.AddTags("PlanApplicationService");
            PlanRepository.AddTags("PlanRepository");


            styles.Add(new ElementStyle("DomainLayer") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("CardController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PlanApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringDomainModel") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightStatus") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AlertStatusRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PlanRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("LocationRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringStationSystemFacade") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView componentView = viewSet.CreateComponentView(MedicalAppointment, "Components", "Component Diagram for DocSeeker");
            componentView.PaperSize = PaperSize.A4_Landscape;

            componentView.Add(landingPage);
            componentView.Add(database);

            componentView.Add(user);
            componentView.Add(professional);
            componentView.Add(soporte);
            componentView.AddAllComponents();

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);


        }
    }
}